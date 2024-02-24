using Confluent.Kafka;
using Microsoft.IdentityModel.Tokens;
using Outbox.Application;
using Outbox.Persistence;
using Outbox.Shared;
using Outbox.Shared.Abstractions;
using Outbox.Shared.Configs;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.ApplicationServicesInjection();

builder.Services.PersistenceServicesInjection();

var _sp = builder.Services.BuildServiceProvider();
var _configuration = _sp.GetRequiredService<IConfiguration>();

KafkaProducerConfig kafkaProducerConfig = new()
{
    Acks = Acks.All,
    ClientId = Dns.GetHostName(),
    TopicPartitionsNumber = 50,
    BootstrapServers = _configuration["KafkaProducerConfig:BootstrapServers"],
    Host = _configuration["KafkaProducerConfig:Host"],
    Topic = _configuration["KafkaProducerConfig:Topic"],
};

builder.Services.AddSingleton<IEventBus>(sp =>
{
    return new EventBusKafka(new() { Connection = kafkaProducerConfig, ConnectionRetryCount = 5, DefaultTopicName = "Outbox", EventBusType = EventBusType.Kafka, EventNameSuffix = "IntegrationEvent", SubscriberClientAppName = "OrderAPI" }, sp, true);
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
