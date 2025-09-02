using Infrastracture.Data;
using Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.config
{
    public static class SeedExtention
    {
        public async static Task SeedOrdersAsync(this MyDbContext context)
        {
            if (!await context.orders.AnyAsync())
            {
                var customer = await context.Costumers.FirstOrDefaultAsync();
                var products = await context.products.Take(2).ToListAsync();

                if (customer == null || !products.Any())
                    return;

                var order = new Order
                {
                    CosumerId = customer.Id,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false,
                    orderItems = new List<OrderItem>
            {
                new OrderItem
                {
                    ProductId = products[0].Id,
                    Quantity = 2,
                    Price = products[0].Price
                },
                new OrderItem
                {
                    ProductId = products[1].Id,
                    Quantity = 1,
                    Price = products[1].Price
                }
            }
                };

                await context.orders.AddAsync(order);
                await context.SaveChangesAsync();
            }
        }

        public async static Task SeedCustomersAsync(this MyDbContext context)
        {
            if (!await context.Costumers.AnyAsync())
            {
                var customers = new List<Costumer>
        {
            new Costumer
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main Street, Kano",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Costumer
            {
                Name = "Mary Johnson",
                Email = "mary.j@example.com",
                Address = "45 Market Road, Kaduna",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Costumer
            {
                Name = "Aliyu Musa",
                Email = "aliyu.m@example.com",
                Address = "78 Independence Way, Katsina",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Costumer
            {
                Name = "Grace Williams",
                Email = "grace.w@example.com",
                Address = "10 Sabon Gari Road, Jigawa",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Costumer
            {
                Name = "Emeka Obi",
                Email = "emeka.obi@example.com",
                Address = "55 Central Market, Kano",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        };

                await context.Costumers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
            }
        }

        public async static Task SeedProductsAsync(this MyDbContext context)
        {
            if (!await context.products.AnyAsync())
            {
                var products = new List<Product>
        {
            new Product
            {
                Name = "Maize (50kg Bag)",
                Description = "High quality yellow maize",
                Price = 25000,
                StockQuantity = 100,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Name = "Rice (50kg Bag)",
                Description = "Premium long grain rice",
                Price = 32000,
                StockQuantity = 80,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Name = "Soybeans (100kg Bag)",
                Description = "Organic soybeans for food & feed",
                Price = 45000,
                StockQuantity = 50,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Name = "Fertilizer (50kg Bag)",
                Description = "NPK 20-10-10 fertilizer",
                Price = 15000,
                StockQuantity = 200,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Product
            {
                Name = "Pesticide (5L)",
                Description = "Effective for crop protection",
                Price = 8000,
                StockQuantity = 120,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        };

                await context.products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

    }
}
