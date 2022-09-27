using Dapper;
using Discount.API.Entities;
using System.Data;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IDbConnection _dbConnection;

        public DiscountRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentException(nameof(dbConnection));
        }

        public async Task<bool> CreateDiscount(Cupon cupon)
        {
            var affected =
                await _dbConnection.ExecuteAsync
                    ("INSERT INTO Cupon (ProductName, Description, Amount) " +
                    "VALUES (@ProductName, @Description, @Amount)", new { cupon.ProductName, cupon.Description, cupon.Amount });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<Cupon> GetDiscount(string productName)
        {
            var cupon = await _dbConnection.QueryFirstOrDefaultAsync<Cupon>
                ("SELECT * FROM Cupon WHERE ProductName = @ProductName", new { ProductName = productName });


            return cupon;
        }

        public async Task<bool> UpdateDiscount(Cupon cupon)
        {
            throw new NotImplementedException();
        }
    }
}
