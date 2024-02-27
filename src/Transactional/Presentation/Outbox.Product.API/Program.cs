using Outbox.Domain.Entities;
using Outbox.Product.API.Events.EventHandlers;
using Outbox.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var _sp = builder.Services.BuildServiceProvider();
var _configuration = _sp.GetRequiredService<IConfiguration>();

builder.Services.AddKafkaConsumer<string, OrderOutbox, OrderCreatedIntegrationEventHandler>(p =>
{
    p.Topic = _configuration["KafkaConsumerConfig:Topic"];
    p.GroupId = _configuration["KafkaConsumerConfig:ConsumerGroupName"];
    p.BootstrapServers = _configuration["KafkaConsumerConfig:Host"];
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
