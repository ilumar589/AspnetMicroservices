using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        Task IBasketRepository.DeleteBasket(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basketJson = await _redisCache.GetStringAsync(userName);

            if (String.IsNullOrEmpty(basketJson))
            {
                return new ShoppingCart
                {
                    UserName = String.Empty,
                    Items = Enumerable.Empty<ShoppingCartItem>().ToList(),
                    FoundInCache = false
                };
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
        }

        Task<ShoppingCart> IBasketRepository.UpdateBasket(ShoppingCart basket)
        {
            throw new NotImplementedException();
        }
    }
}
