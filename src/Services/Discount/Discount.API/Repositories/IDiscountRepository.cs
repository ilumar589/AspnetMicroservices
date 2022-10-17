using Discount.API.Entities;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Cupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Cupon cupon);
        Task<bool> UpdateDiscount(Cupon cupon);
        Task<bool> DeleteDiscount(string productName);
    }
}
