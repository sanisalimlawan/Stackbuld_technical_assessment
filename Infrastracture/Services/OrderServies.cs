using Core.DTOs;
using Core.Interface;
using Infrastracture.Data;
using Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.OrderDTO;

namespace Infrastracture.Services
{
    public class OrderServies : IOrderRepo
    {
        private readonly ILogger<OrderServies> _logger;
        private readonly IGenericRepo<Order> _oderRepo;
        private readonly IGenericRepo<Costumer> _customerRepo;
        private readonly IGenericRepo<Product> _product;
        private readonly MyDbContext _db;
        public OrderServies(ILogger<OrderServies> logger, IGenericRepo<Order> orderepo, IGenericRepo<Costumer> costumer, MyDbContext db, IGenericRepo<Product> product)
        {
            _logger = logger;
            _oderRepo = orderepo;
            _customerRepo = costumer;
            _db = db;
            _product = product;
        }
        public async Task<ApiResponse> PlaceOrder(OrderDTO.PlaceOrderRequest dto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var productIds = dto.Items.Select(i => i.ProductId).ToList();
                var products = (await _product.FindAsync(pro => productIds.Contains(pro.Id))).ToList();
                var productDic = products.ToDictionary(p => p.Id);
                foreach (var item in dto.Items)
                {
                    if (!productDic.ContainsKey(item.ProductId))
                    {
                        return new ApiResponse((int)HttpStatusCode.NotFound,
                            $"Product with ID '{item.ProductId}' not found.", null, false);
                    }
                    var product = productDic[item.ProductId];
                    
                    if (product.StockQuantity < item.Quantity)
                    {
                        //conflict: Gracefully Fail the Order
                        return new ApiResponse(
                                (int)HttpStatusCode.Conflict,
                                 $"Not enough stock for product '{product.Name}'",
                                 null,
                                 false
                        );
                    }
                }
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    CosumerId = dto.UserId
                };

                foreach (var item in dto.Items)
                {
                    var product = productDic[item.ProductId];
                    product.StockQuantity -= item.Quantity;
                    order.orderItems.Add(new OrderItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        OrderId = order.Id,
                        Price = product.Price

                    });
                }
                await _oderRepo.AddAsync(order);
                await _oderRepo.SaveChangesAsync();
                await transaction.CommitAsync();
                var responseDTO = MapOrderResponse(order);
                return new ApiResponse((int)HttpStatusCode.Created, "Order Place Sucessfully", responseDTO, true);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency conflict while placing an order for user {UserId}", dto.UserId);

                return new ApiResponse(
                    (int)HttpStatusCode.Conflict,
                    "The product stock was updated by another order. Please try again.",
                    null,
                    false
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while placing an order for user {UserId}", dto.UserId);

                return new ApiResponse(
                    (int)HttpStatusCode.InternalServerError,
                    "An unexpected error occurred. Please try again later.",
                    null,
                    false
                );
            }
        }
        public async Task<ApiResponse> DeleteProductAsync(Guid id)
        {
            try
            {
                var product = await _oderRepo.GetByIdAsync(i => i.Id == id);
                if (product == null)
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Order Not Found", null, false);
                _oderRepo.Remove(product);
                await _oderRepo.SaveChangesAsync();
                return new ApiResponse((int)HttpStatusCode.OK, "Order Deleted Successfully", null, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Deleting product with Id {id}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> GetAllOrders()
        {
            try
            {
                var orders = await _oderRepo.GetAllAsync("Costumer", "orderItems.Product");
                if (orders == null || !orders.Any())
                {
                    return new ApiResponse((int)HttpStatusCode.NotFound, "NO Data Found ", null, false);
                }
                var formatedData = orders.Select(x => MapOrderResponse(x));
                return new ApiResponse((int)HttpStatusCode.OK, "Oders Retreive Successfully", formatedData, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while  retreiving the Orders");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }
        public async Task<ApiResponse> GetOrdersById(Guid Id)
        {
            try
            {
                var order = await _oderRepo.GetByIdAsync(x => x.Id == Id, "Costumer", "orderItems.Product");
                if (order == null)
                {
                    return new ApiResponse((int)HttpStatusCode.NotFound, $"Order Not Found With Id {Id} ", null, false);
                }
                var formatedData = MapOrderResponse(order);
                return new ApiResponse((int)HttpStatusCode.OK, "Oders Retreive Successfully", formatedData, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while retreiving the Order");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }


        public async Task<ApiResponse> GetOrdersByCustomerId(Guid userId)
        {
            try
            {
                // First check if the customer exists
                var customerExists = await _customerRepo.GetByIdAsync(x => x.Id == userId);
                if (customerExists == null)
                {
                    return new ApiResponse(
                        (int)HttpStatusCode.NotFound,
                        $"Customer with ID {userId} not found.",
                        null,
                        false
                    );
                }

                // Fetch orders
                var orders = await _oderRepo.FindAsync(o => o.CosumerId == userId);

                if (orders == null || !orders.Any())
                {
                    return new ApiResponse(
                        (int)HttpStatusCode.NoContent,
                        $"Customer with ID {userId} has no orders.",
                        null,
                        true
                    );
                }

                var formattedData = orders.Select(o => MapOrderResponse(o));

                return new ApiResponse(
                    (int)HttpStatusCode.OK,
                    "Orders retrieved successfully.",
                    formattedData,
                    true
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving the orders");
                return new ApiResponse(
                    (int)HttpStatusCode.InternalServerError,
                    "An unexpected error occurred, please try again later.",
                    null,
                    false
                );
            }
        }

        private OrderResponse MapOrderResponse(Order dto)
        {
            return new OrderResponse
            {
                OrderId = dto.Id,
                CostumerId = dto.CosumerId,
                CreatedAt = dto.CreatedAt,
                IsDeleted = dto.IsDeleted,
                UpdatedAt = dto.UpdatedAt,
                costumer = dto.Costumer != null ? new CostumerDTO.CostumerResponse
                {
                    Id = dto.Costumer.Id,
                    Address = dto.Costumer.Address,
                    Name = dto.Costumer.Name,
                    Email = dto.Costumer.Email
                } :null,
                OrderItems = dto.orderItems.Select(it => new OrderItemResponse
                {
                    ProductId = it.ProductId,
                    Price = it.Price,
                    product = it.Product != null ? new ProductDTO.ProductResponseDTO
                    {
                        Id = it.Product.Id,
                        Name = it.Product.Name,
                        Description = it.Product.Description,
                        Price = it.Product.Price,

                    } : null,
                    Quantity = it.Quantity,
                }).ToList(),

            };
        }


    }
}
