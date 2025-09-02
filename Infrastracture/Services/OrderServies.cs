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
        private readonly IGenericRepo<OrderItem> _orderitem;
        private readonly IGenericRepo<Product> _product;
        private readonly MyDbContext _db;
        public OrderServies(ILogger<OrderServies> logger, IGenericRepo<Order> orderepo, IGenericRepo<OrderItem> orderItem, MyDbContext db, IGenericRepo<Product> product)
        {
            _logger = logger;
            _oderRepo = orderepo;
            _orderitem = orderItem;
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
                    return new ApiResponse((int)HttpStatusCode.NotFound, "NO Data Found ", null, false);
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
