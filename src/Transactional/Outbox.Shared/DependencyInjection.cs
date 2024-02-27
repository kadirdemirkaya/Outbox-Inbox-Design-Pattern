using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Outbox.Shared.Abstractions;
using Outbox.Shared.Consumer;
using Outbox.Shared.Producer;

namespace Outbox.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddKafkaMessageBus(this IServiceCollection serviceCollection)
          => serviceCollection.AddSingleton(typeof(IMessageBus<,>), typeof(MessageBus<,>));

        public static IServiceCollection AddKafkaConsumer<TK, TV, THandler>(this IServiceCollection services,
            Action<KafkaConsumerConfig<TK, TV>> configAction) where THandler : class, IKafkaHandler<TK, TV>
        {
            services.AddScoped<IKafkaHandler<TK, TV>, THandler>();

            services.AddHostedService<BackGroundConsumer<TK, TV>>();

            services.Configure(configAction);

            return services;
        }

        public static IServiceCollection AddKafkaProducer<TK, TV>(this IServiceCollection services,
            Action<KafkaProducerConfig<TK, TV>> configAction)
        {
            services.AddConfluentKafkaProducer<TK, TV>();

            services.AddSingleton<KafkaProducer<TK, TV>>();

            services.Configure(configAction);

            return services;
        }

        private static IServiceCollection AddConfluentKafkaProducer<TK, TV>(this IServiceCollection services)
        {
            services.AddSingleton(
                sp =>
                {
                    var config = sp.GetRequiredService<IOptions<KafkaProducerConfig<TK, TV>>>();

                    var builder = new ProducerBuilder<TK, TV>(config.Value).SetValueSerializer(new KafkaSerializer<TV>());

                    return builder.Build();
                });

            return services;
        }
    }
}
