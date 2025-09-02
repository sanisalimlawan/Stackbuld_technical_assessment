using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class ProductDTO
    {
        public class CreateProductReuest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
        }
        public class RestockRequest
        {
            public int Quantity { get; set; }
        }

        public class UpdateProductDTO
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
        }
        public class ProductResponseDTO
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }
    }
}
