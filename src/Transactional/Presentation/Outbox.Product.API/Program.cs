using Confluent.Kafka;
using Outbox.Product.API.Events.EventHandlers;
using Outbox.Shared;
using Outbox.Shared.Abstractions;
using Outbox.Shared.Configs;
using Outbox.Shared.IntegrationEvents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var _sp = builder.Services.BuildServiceProvider();
var _configuration = _sp.GetRequiredService<IConfiguration>();

KafkaConsumerConfig kafkaConsumerConfig = new()
{
    AutoOffsetReset = AutoOffsetReset.Earliest,
    ConsumerGroupName = _configuration["KafkaConsumerConfig:ConsumerGroupName"],
    Host = _configuration["KafkaConsumerConfig:Host"],
    Topic = _configuration["KafkaConsumerConfig:Topic"],
};

builder.Services.AddSingleton<IEventBus>(sp =>
{
    return new EventBusKafka(new() { ConnectionRetryCount = 5, DefaultTopicName = "Outbox", EventBusType = EventBusType.Kafka, EventNameSuffix = "IntegrationEvent", SubscriberClientAppName = "ProductId", Connection = kafkaConsumerConfig }, sp, true);
});

builder.Services.AddTransient<OrderCreatedIntegrationEventHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
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
