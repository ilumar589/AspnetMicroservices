namespace Basket.API.Entities
{
    public record struct ShoppingCart(
        string UserName,
        List<ShoppingCartItem> Items,
        bool FoundInCache = true)
    {
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                Items.ForEach(item => totalPrice += item.Price * item.Quantity);
                return totalPrice;
            }
        }
    }
}
