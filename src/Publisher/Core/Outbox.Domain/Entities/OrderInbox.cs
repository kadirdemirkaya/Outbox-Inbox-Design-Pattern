using Outbox.Domain.Entities.Base;
using Outbox.Domain.Enums;

namespace Outbox.Domain.Entities
{
    public class OrderInbox : BaseEntity
    {
        public string @Type { get; set; }
        public string Payload { get; set; }
        public OrderOutboxStatus OrderOutboxStatus { get; set; }

        public OrderInbox()
        {
            
        }
        public OrderInbox(string type, string payload, OrderOutboxStatus orderOutboxStatus)
        {
            Id = Guid.NewGuid();
            Type = type;
            Payload = payload;
            OrderOutboxStatus = orderOutboxStatus;
        }

        public OrderInbox(Guid id, string type, string payload, OrderOutboxStatus orderOutboxStatus)
        {
            Id = id;
            Type = type;
            Payload = payload;
            OrderOutboxStatus = orderOutboxStatus;
        }

        public static OrderInbox CreateOrderInbox(string type, string payload, OrderOutboxStatus orderOutboxStatus)
            => new(type, payload, orderOutboxStatus);

        public static OrderInbox CreateOrderInbox(Guid id, string type, string payload, OrderOutboxStatus orderOutboxStatus)
            => new(id, type, payload, orderOutboxStatus);

    }
}
