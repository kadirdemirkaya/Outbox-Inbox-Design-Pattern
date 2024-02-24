namespace Outbox.Application.Dtos
{
    public class CreateOrderDto
    {
        public int Quantity { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}
