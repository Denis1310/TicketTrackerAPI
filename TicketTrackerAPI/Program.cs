using TicketTrackerAPI.Entities;
using TicketTrackerAPI.Features.Notificators;
using TicketTrackerAPI.Models;
using TicketTrackerAPI.Repositories;
using TicketTrackerAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Ticket).Assembly));

builder.Services.AddSingleton<INotificationInMemoryRepository, NotificationInMemoryRepository>();
builder.Services.AddSingleton<ITicketInMemoryRepository, TicketInMemoryRepository>();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INotificationService<EmailContent>, EmailService>();
builder.Services.AddTransient<INotificationService<SmsContent>, SmsService>();
builder.Services.AddTransient<INotificationService<PushContent>, PushService>();
builder.Services.AddTransient(typeof(NotificationProcessor<>));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder.Logging.AddConsole();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
