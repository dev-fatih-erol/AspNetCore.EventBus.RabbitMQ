using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.EventBus.RabbitMQ
{
    public static class EventBusRabbitMQExtentions
    {
        public static void AddEventBusRabbitMQ(this IServiceCollection services, Action<EventBusRabbitMQOptions> configureOptions)
        {
            var options = new EventBusRabbitMQOptions();

            configureOptions(options);

            services.Configure(configureOptions);

            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();

            services.AddEventBus<EventBusRabbitMQ>();
        }
    }
}