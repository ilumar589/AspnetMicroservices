namespace Discount.API.Entities
{
    public class Cupon
    {
        public long Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Amount { get; set; } = 0;
    }
}
