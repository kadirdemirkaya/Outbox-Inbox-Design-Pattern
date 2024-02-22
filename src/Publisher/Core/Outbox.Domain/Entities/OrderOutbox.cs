using Outbox.Domain.Entities.Base;
using Outbox.Domain.Enums;

namespace Outbox.Domain.Entities
{
    public class OrderOutbox : BaseEntity
    {
        public string @Type { get; set; }
        public string Payload { get; set; }
        public OrderOutboxStatus OrderOutboxStatus { get; set; }

        public OrderOutbox()
        {
            
        }
        public OrderOutbox(string type, string payload, OrderOutboxStatus orderOutboxStatus)
        {
            Id = Guid.NewGuid();
            Type = type;
            Payload = payload;
            OrderOutboxStatus = orderOutboxStatus;
        }

        public OrderOutbox(Guid id, string type, string payload, OrderOutboxStatus orderOutboxStatus)
        {
            Id = id;
            Type = type;
            Payload = payload;
            OrderOutboxStatus = orderOutboxStatus;
        }

        public static OrderOutbox CreateOrderOutbox(string type, string payload, OrderOutboxStatus orderOutboxStatus)
            => new(type, payload, orderOutboxStatus);
        public static OrderOutbox CreateOrderOutbox(Guid id, string type, string payload, OrderOutboxStatus orderOutboxStatus)
        => new(id, type, payload, orderOutboxStatus);
    }
}
