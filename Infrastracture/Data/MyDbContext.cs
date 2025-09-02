using Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }
        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
        {
            public MyDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

                // Build the configuration from appsettings.json or environment variables
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json") // Make sure your appsettings.json is in the correct directory
                    .Build();

                // Set up the DbContext to use the connection string from appsettings.json
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("StackbuldConnection"));

                return new MyDbContext(optionsBuilder.Options);
            }
        }
        public DbSet<Product> products { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> ordersItem { get; set; }
        public DbSet<Costumer> Costumers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType).Property<DateTime?>("UpdatedAt")
                        .HasColumnName("updated_at");

                    modelBuilder.Entity(entityType.ClrType).Property<bool>("IsActive")
                        .HasColumnName("is_active")
                        .HasDefaultValue(true);

                    modelBuilder.Entity(entityType.ClrType).Property<bool>("IsDeleted")
                        .HasColumnName("is_deleted")
                        .HasDefaultValue(false);
                    // SOFT DELET
                    var param = Expression.Parameter(entityType.ClrType);
                    var IsdeletedPop = Expression.Property(param, "IsDeleted");
                    var comapreExpr = Expression.Equal(IsdeletedPop, Expression.Constant(false));
                    var lamda = Expression.Lambda(comapreExpr, param);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lamda);
                }
            }
            // product config
            modelBuilder.Entity<Product>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.ToTable("products");
                builder.Property(p => p.Name)
                       .HasColumnName("name")
                       .IsRequired()
                       .HasMaxLength(100);
                builder.Property(p => p.Description)
                       .HasColumnName("description")
                       .HasMaxLength(500);
                builder.Property(p => p.Price)
                       .HasColumnName("price")
                       .IsRequired()
                       .HasColumnType("decimal(18,2)");
                builder.Property(p => p.StockQuantity)
                        .HasColumnName("stock_quantity")
                        .IsRequired();
                builder.Property(p => p.RowVersion)
                       .IsRowVersion()
                       .IsConcurrencyToken();
            });
            // order config
            modelBuilder.Entity<Order>(builder =>
            {
                builder.HasKey(o => o.Id);
                builder.ToTable("orders");
                builder.HasMany(o => o.orderItems)
                       .WithOne(o => o.Order)
                       .HasForeignKey(o => o.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(o => o.Costumer)
                       .WithMany(o => o.Orders)
                       .HasForeignKey(o => o.CosumerId);
            });
            // orderitem config
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.HasKey(oi => oi.Id);
                builder.ToTable("order_items");
                builder.Property(oi => oi.Quantity)
                       .HasColumnName("quantity")
                       .IsRequired();

                builder.Property(oi => oi.Price)
                       .HasColumnName("price")
                       .HasColumnType("decimal(18,2)");

                // Relationship: OrderItem -> Product
                builder.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            // costumer config
            modelBuilder.Entity<Costumer>(builder =>
            {
                builder.HasKey(c => c.Id);
                builder.ToTable("Costumers");
                builder.Property(c => c.Email)
                       .HasColumnName("email")
                       .IsRequired().HasMaxLength(50);
                builder.Property(c => c.Name)
                       .HasColumnName("name")
                       .IsRequired();
                builder.Property(c => c.Address)
                       .HasColumnName("address");
            });
        }
    }

}
