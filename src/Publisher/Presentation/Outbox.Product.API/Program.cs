using Microsoft.AspNetCore.Mvc.Formatters;
using Outbox.Product.API.Events.EventHandlers;
using Outbox.Shared;
using Outbox.Shared.Abstractions;
using Outbox.Shared.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEventBus>(sp =>
{
    return new EventBusRabbitMQ(new() { SubscriberClientAppName = "ProductAPI", EventNameSuffix = "IntegrationEvent", EventBusType = EventBusType.RabbitMQ, DefaultTopicName = "Outbox", ConnectionRetryCount = 5 }, sp);
});

builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var sp = builder.Services.BuildServiceProvider();
var eventBus = sp.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

app.Run();
