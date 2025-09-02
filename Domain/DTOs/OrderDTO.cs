using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.CostumerDTO;
using static Core.DTOs.ProductDTO;

namespace Core.DTOs
{
    public class OrderDTO
    {
        // Request DTO for placing an order
        public class PlaceOrderRequest
        {
            public Guid UserId { get; set; }  
            public List<OrderItemRequest> Items { get; set; } = new List<OrderItemRequest>();
        }
        public class OrderItemRequest
        {
            public Guid ProductId { get; set; }
            public int Quantity { get; set; }
        }
        // Response DTO for returning order details
        public class OrderResponse
        {
            public Guid OrderId { get; set; }
            public Guid CostumerId { get; set; }
            public CostumerResponse costumer { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public bool IsDeleted { get; set; }
            public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
        }

        // Each item in the response
        public class OrderItemResponse
        {
            public Guid ProductId { get; set; }
            public ProductResponseDTO product { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; } // price at the time of order
        }
    }
}

