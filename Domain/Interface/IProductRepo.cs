using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.ProductDTO;

namespace Core.Interface
{
    public interface IProductRepo
    {
        public Task<ApiResponse> CreateProductAsync(CreateProductReuest dto);
        public Task<ApiResponse> GetProductByIdAsync(Guid id);
        public Task<ApiResponse> GetAllProductsAsync();
        public Task<ApiResponse> UpdateProductAsync(UpdateProductDTO dto,Guid id);
        public Task<ApiResponse> DeleteProductAsync(Guid id);
        public Task<ApiResponse> RestockProductAsync(Guid productId, int quantity);


    }
}
