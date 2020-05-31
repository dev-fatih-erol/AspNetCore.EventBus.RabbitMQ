namespace AspNetCore.EventBus.RabbitMQ
{
    public class EventBusRabbitMQOptions
    {
        public string HostName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string QueueName { get; set; } = "event_bus_queue";

        public string BrokerName { get; set; } = "event_bus";

        public int RetryCount { get; set; } = 5;
    }
}