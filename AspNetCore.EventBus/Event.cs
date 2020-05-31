using System;

namespace AspNetCore.EventBus
{
    public class Event
    {
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}