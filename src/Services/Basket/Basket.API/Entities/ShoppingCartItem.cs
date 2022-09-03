namespace Basket.API.Entities
{
    public record struct ShoppingCartItem(int Quantity, string Color, decimal Price, string ProductId, string ProductName)
    {
    }
}
