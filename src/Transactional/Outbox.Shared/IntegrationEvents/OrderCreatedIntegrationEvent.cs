namespace Outbox.Shared.IntegrationEvents
{
    public class OrderCreatedIntegrationEvent 
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public OrderCreatedIntegrationEvent(Guid id, int quantity, string description, double price)
        {
            Id = id;
            Quantity = quantity;
            Description = description;
            Price = price;
        }
    }
}
