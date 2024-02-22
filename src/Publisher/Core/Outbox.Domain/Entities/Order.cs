using Outbox.Domain.Entities.Base;

namespace Outbox.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public Order()
        {
            
        }
        public Order(int quantity, string description, double price)
        {
            Id = Guid.NewGuid();
            Quantity = quantity;
            Description = description;
            Price = price;
        }

        public Order(Guid id, int quantity, string description, double price)
        {
            Id = id;
            Quantity = quantity;
            Description = description;
            Price = price;
        }

        public static Order CreateOrder(int quantity, string description, double price)
            => new(quantity, description, price);
        public static Order CreateOrder(Guid id, int quantity, string description, double price)
            => new(id, quantity, description, price);
    }
}
