using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Shared;
using Outbox.Shared.Abstractions;
using Outbox.Shared.Configs;
using Outbox.Transaction.Log.Service.Services;

class Program
{
    static void Main(string[] args)
    {
        var host = CreatedHostBuilder(args).Build();
        var serviceProvider = host.Services;

        host.Run();
    }

    public static IHostBuilder CreatedHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            var sp = services.BuildServiceProvider();
            //var _configuration = sp.GetRequiredService<IConfiguration>();

            IConfiguration _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .SetBasePath("C:/Users/Casper/Desktop/GitHub Projects/OutboxInboxDesignPattern/src/Transactional/TransactionLog/Outbox.Transaction.Log.Service")
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            KafkaConsumerConfig kafkaConsumerConfig = new()
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ConsumerGroupName = _configuration["KafkaConsumerConfig:ConsumerGroupName"],
                Host = _configuration["KafkaConsumerConfig:Host"],
                Topic = _configuration["KafkaConsumerConfig:Topic"],
            };

            services.AddScoped<IEventBus>(sp =>
            {
                return new EventBusKafka(new() { ConnectionRetryCount = 5, DefaultTopicName = "Outbox", EventBusType = EventBusType.Kafka, EventNameSuffix = "IntegrationEvent", SubscriberClientAppName = "CONSUMESERVICENAME", Connection = kafkaConsumerConfig }, sp);
            });

            services.AddHostedService<ConsumerBackgroundService>();

        });
}



