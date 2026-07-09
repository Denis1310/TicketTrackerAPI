using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using TicketTrackerAPI.Controllers;
using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Entities.enums;
using TicketTrackerAPI.Features.Notificators;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Models.DTOs;
using TicketTrackerAPI.Repositories;
using TicketTrackerAPI.Services;

namespace TicketTrackerAPI.Tests.Handlers;

public class TicketsControllerTest
{
    readonly TicketsController _controller;
    readonly IMediator _mediator;

    readonly ITicketInMemoryRepository _ticketRepoMock = Substitute.For<ITicketInMemoryRepository>();
    readonly INotificationInMemoryRepository _notificationRepoMock = Substitute.For<INotificationInMemoryRepository>();
    readonly INotificationService<EmailContent> _emailServiceMock = Substitute.For<INotificationService<EmailContent>>();
    readonly INotificationService<SmsContent> _smsServiceMock = Substitute.For<INotificationService<SmsContent>>();
    readonly INotificationService<PushContent> _pushServiceMock = Substitute.For<INotificationService<PushContent>>();

    public TicketsControllerTest()
    {
        var services = new ServiceCollection();

        // required for MediatR.
        services.AddLogging();

        services.AddSingleton(_ticketRepoMock);
        services.AddSingleton(_notificationRepoMock);
        services.AddTransient<IUserService, UserService>();
        services.AddTransient(typeof(NotificationProcessor<>));
        services.AddSingleton(_emailServiceMock);
        services.AddSingleton(_smsServiceMock);
        services.AddSingleton(_pushServiceMock);

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Ticket).Assembly));

        var provider = services.BuildServiceProvider();

        _mediator = provider.GetRequiredService<IMediator>();

        _controller = new TicketsController(_mediator);
    }

    [Fact]
    public async Task CreateTicket_WithValidModel_ShouldCreateNewTicketWithPendingNotifications()
    {
        // arrange.
        var request = new CreateTicketRequest
        {
            Title = "Test Ticket",
            Priority = Priority.High
        };

        var expectedTypesCount = Enum.GetValues(typeof(Channel)).Length;

        // act.
        var result = await _controller.CreateTicket(request);

        // assert.
        var createdResult = Assert.IsType<CreatedResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        _ticketRepoMock
            .Received(1)
            .AddTicket(Arg.Is<Ticket>(
                t => t.Title == request.Title &&
                t.Priority == request.Priority));

        _notificationRepoMock
            .Received(1)
            .AddRange(Arg.Is<List<Notification>>(
                x => x.Count == expectedTypesCount &&
                x.All(n => n.Status == Status.Pending)));
    }

    [Fact]
    public async Task NotifyTicket_WithExistingTicket_ShouldSendAllNotificationsAndUpdateStatuses()
    {
        // arrange.
        var expectedStatus = Status.Sent;

        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);

        // act.
        var result = await _controller.NotifyTicket(ticket.Id);

        // assert.
        Assert.IsType<OkResult>(result);

        foreach (var notification in createdNotifications)
        {
            _notificationRepoMock
                .Received(1)
                .UpdateNotification(Arg.Is<Notification>(
                    n => n.Channel == notification.Channel &&
                    n.Status == expectedStatus));
        }
    }

    [Fact]
    public async Task NotifyTicket_WithRetry_IncrementsAttempts()
    {
        // arrange.
        var expectedAttempts = 2;
        var expectedStatus = Status.Failed;

        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);
        SetupNotificatinServicesTimeout();

        // act.
        await _controller.NotifyTicket(ticket.Id);

        // assert.
        foreach (var notification in createdNotifications)
        {
            _notificationRepoMock
                .Received(1)
                .UpdateNotification(Arg.Is<Notification>(
                    n => n.Channel == notification.Channel &&
                    n.Attemps == expectedAttempts &&
                    n.Status == expectedStatus));
        }
    }

    [Fact]
    public async Task NotifyTicket_WhenRetryFails_ShouldStoreErrorMessageInLastError()
    {
        // arrange.
        var expectedErrorKeywordPart = "Timeout";

        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);
        SetupNotificatinServicesTimeout();

        // act.
        await _controller.NotifyTicket(ticket.Id);

        // assert.
        _notificationRepoMock
            .UpdateNotification(Arg.Is<Notification>(n =>
                n.LastError != null &&
                n.LastError.Contains(expectedErrorKeywordPart)));
    }

    [Fact]
    public async Task NotifyTicket_WhenRetryLimitExceeded_StopsIncrementAttemts()
    {
        // arrange.
        var maxAttempts = 3;
        var expectedCalls = 6;

        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);
        SetupNotificatinServicesTimeout();

        // act.
        for (int i = 0; i <= maxAttempts + 1; i++)
        {
            await _controller.NotifyTicket(ticket.Id);
        }

        // assert.
        _notificationRepoMock
            .Received(expectedCalls)
            .UpdateNotification(Arg.Any<Notification>());
    }

    [Fact]
    public async Task NotifyTicket_WhenCalledAgain_ShouldNotResendSentNotifications()
    {
        // arrange.
        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);

        foreach (var notification in createdNotifications)
        {
            notification.Status = Status.Sent;
        }

        // act.
        var result = await _controller.NotifyTicket(ticket.Id);

        // assert.
        _notificationRepoMock
            .DidNotReceive()
            .UpdateNotification(Arg.Any<Notification>());

        await _emailServiceMock
            .DidNotReceive()
            .Send(Arg.Any<EmailContent>());

        await _smsServiceMock
            .DidNotReceive()
            .Send(Arg.Any<SmsContent>());

        await _pushServiceMock
            .DidNotReceive()
            .Send(Arg.Any<PushContent>());
    }

    [Fact]
    public async Task NotifyTicket_CalledMultipleTimes_ShouldNotCreateNewNotifications()
    {
        // arrange.
        Ticket ticket;
        List<Notification> createdNotifications;

        SetupTicketWithPendingNotifications(out ticket, out createdNotifications);

        // act.
        await _controller.NotifyTicket(ticket.Id);
        await _controller.NotifyTicket(ticket.Id);

        // assert.
        _notificationRepoMock
            .DidNotReceive()
            .AddRange(Arg.Any<List<Notification>>());
    }

    void SetupTicketWithPendingNotifications(out Ticket ticket, out List<Notification> createdNotifications)
    {
        ticket = new Ticket
        {
            Title = "Test Ticket",
            Priority = Priority.Medium
        };

        createdNotifications = new List<Notification>
        {
            new Notification
            {
                TicketId = ticket.Id,
                Channel = Channel.Email,
                Status = Status.Pending,
                Attemps = 1
            },
            new Notification
            {
                TicketId = ticket.Id,
                Channel = Channel.Sms,
                Status = Status.Pending,
                Attemps = 1
            },
            new Notification
            {
                TicketId = ticket.Id,
                Channel = Channel.Push,
                Status = Status.Pending,
                Attemps = 1
            },
        };

        _ticketRepoMock
            .GetTicketById(ticket.Id)
            .Returns(ticket);

        _notificationRepoMock
            .GetByTicketIdAndChannel(Arg.Any<Guid>(), Channel.Email)
            .Returns(createdNotifications[0]);

        _notificationRepoMock
            .GetByTicketIdAndChannel(Arg.Any<Guid>(), Channel.Sms)
            .Returns(createdNotifications[1]);

        _notificationRepoMock
            .GetByTicketIdAndChannel(Arg.Any<Guid>(), Channel.Push)
            .Returns(createdNotifications[2]);
    }

    void SetupNotificatinServicesTimeout()
    {
        _emailServiceMock.Send(Arg.Any<EmailContent>())
                    .ThrowsAsync(new TimeoutException());

        _smsServiceMock.Send(Arg.Any<SmsContent>())
            .ThrowsAsync(new TimeoutException());

        _pushServiceMock.Send(Arg.Any<PushContent>())
            .ThrowsAsync(new TimeoutException());
    }
}
