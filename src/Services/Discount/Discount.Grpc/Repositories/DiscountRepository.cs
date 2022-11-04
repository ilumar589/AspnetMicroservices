using Dapper;
using Discount.Grpc.Entities;
using System.Data;

namespace Discount.Grpc.Repositories
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
            var affected = await _dbConnection.ExecuteAsync("DELETE FROM Cupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<Cupon> GetDiscount(string productName)
        {
            var cupon = await _dbConnection.QueryFirstOrDefaultAsync<Cupon>
                ("SELECT * FROM Cupon WHERE ProductName = @ProductName", new { ProductName = productName });


            return cupon;
        }

        public async Task<bool> UpdateDiscount(Cupon cupon)
        {
            var affected = await _dbConnection.ExecuteAsync
                ("UPDATE Cupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { cupon.ProductName, cupon.Description, cupon.Amount, cupon.Id });

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}
