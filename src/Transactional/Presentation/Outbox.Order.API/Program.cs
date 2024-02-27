using Outbox.Application;
using Outbox.Domain.Entities;
using Outbox.Persistence;
using Outbox.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.ApplicationServicesInjection();

builder.Services.PersistenceServicesInjection();

var _sp = builder.Services.BuildServiceProvider();
var _configuration = _sp.GetRequiredService<IConfiguration>();

builder.Services.AddKafkaMessageBus();

builder.Services.AddKafkaProducer<string, OrderOutbox>(p =>
{
    p.Topic = _configuration["KafkaProducerConfig:Topic"];
    p.BootstrapServers = _configuration["KafkaProducerConfig:Host"];
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
