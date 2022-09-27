namespace Discount.API.Entities
{
    public record struct Cupon(int Id, string ProductName, string Description, int Amount)
    {
    }
}
