using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Job.Service.Jobs;
using Outbox.Shared;
using Outbox.Shared.Abstractions;
using Quartz;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddQuartz(configurator =>
        {
            configurator.UseMicrosoftDependencyInjectionJobFactory();

            JobKey jobKey = new("OrderOutboxPublishJob");

            configurator.AddJob<OrderOutboxPublishJob>(options => options.WithIdentity(jobKey));

            TriggerKey triggerKey = new("OrderOutboxPublishTrigger");

            configurator.AddTrigger(options => options.ForJob(jobKey)
                        .WithIdentity(triggerKey)
                        .StartAt(DateTime.UtcNow)
                        .WithSimpleSchedule
                        (
                            builder => builder.WithIntervalInSeconds(30)
                                              .RepeatForever()
                        ));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.AddScoped<IEventBus>(sp =>
        {
            return new EventBusRabbitMQ(new() { ConnectionRetryCount = 5, DefaultTopicName = "Outbox", EventBusType = EventBusType.RabbitMQ, EventNameSuffix = "IntegrationEvent", SubscriberClientAppName = "OrderAPI" }, sp);
        });

    })
    .Build();

await host.RunAsync();