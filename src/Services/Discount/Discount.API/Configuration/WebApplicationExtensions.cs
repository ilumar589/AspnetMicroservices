using Npgsql;
using System.Data;

namespace Discount.API.Configuration
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MigrateDatabase(this WebApplication app, int? retry = 0)
        {
            int retryForAvailibility = retry.Value;

            using var scope = app.Services.CreateScope();
            var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            try
            {
                logger.LogInformation("Migrating postgresql database.");
                dbConnection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = (NpgsqlConnection)dbConnection
                };

                command.CommandText = "DROP TABLE IF EXISTS Cupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Cupon(Id BIGSERIAL PRIMARY KEY,
                                                           ProductName VARCHAR(255) NOT NULL,
                                                           Description TEXT,
                                                           Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Cupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Cupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();

                logger.LogInformation("Migrated postgresql database.");

            }
            catch (NpgsqlException e)
            {
                logger.LogError(e, "An error occured while migrating the postgresql database");

                if (retryForAvailibility < 50)
                {
                    retryForAvailibility++;
                    Thread.Sleep(2000);
                    MigrateDatabase(app, retryForAvailibility);
                }
            }

            return app;
        }
    }
}
