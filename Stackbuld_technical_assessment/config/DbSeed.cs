using Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.config
{
    public class DbSeed : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public DbSeed(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeed>>();
            var MyDbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

            try
            {
                logger.LogInformation("Applying Migration");
                await MyDbContext.Database.MigrateAsync(cancellationToken);
                logger.LogInformation("Migration Apply successfully");

                logger.LogInformation("Seeding Costumers");
                await MyDbContext.SeedCustomersAsync();
                logger.LogInformation(" Costumers Seed successfully");

                logger.LogInformation("Seeding Products");
                await MyDbContext.SeedProductsAsync();
                logger.LogInformation("Products seed successfully");

                logger.LogInformation("Seeding Orders");
                await MyDbContext.SeedOrdersAsync();
                logger.LogInformation("Orders Seed Successfully");


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while applying Access DB Migration!");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
