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

        async Task IBasketRepository.DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basketJson = await _redisCache.GetStringAsync(userName);

            if (String.IsNullOrEmpty(basketJson))
            {
                return new ShoppingCart
                {
                    UserName = userName,
                    Items = Enumerable.Empty<ShoppingCartItem>().ToList(),
                    FoundInCache = false
                };
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(basketJson);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

            return await GetBasket(basket.UserName);
        }
    }
}
