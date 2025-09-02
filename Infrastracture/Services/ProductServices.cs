using Core.DTOs;
using Core.Interface;
using Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.ProductDTO;

namespace Infrastracture.Services
{
    public class ProductServices : IProductRepo
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<OrderItem> _orderItemRepo;

        private readonly ILogger<ProductServices> _logger;
        public ProductServices(IGenericRepo<Product> genericRepo,ILogger<ProductServices> logger,IGenericRepo<OrderItem> orderItemRepo)
        {
            _productRepo = genericRepo;
            _logger = logger;
            _orderItemRepo = orderItemRepo;
        }
        public async Task<ApiResponse> CreateProductAsync(ProductDTO.CreateProductReuest dto)
        {
            try
            {
                var conflict = (await _productRepo.FindAsync(product => dto.Name == product.Name)).FirstOrDefault();
                if (conflict != null)
                {
                    return new ApiResponse((int)HttpStatusCode.Conflict,
                        $"Product with name '{dto.Name}' already exists",
                        null,
                        false);
                };
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    StockQuantity = dto.StockQuantity,
                    CreatedAt = DateTime.UtcNow
                };
                await _productRepo.AddAsync(product);
                await _productRepo.SaveChangesAsync();
                var formatedData = MapProductResponse(product);
                return new ApiResponse((int)HttpStatusCode.Created, "Product Added Successfully", formatedData, true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while creating product with name {dto.Name}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> DeleteProductAsync(Guid id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(i => i.Id == id);
                if (product == null)
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Product Not Found", null, false);
                //Ensure Data Integarity Check if product has orders
                //bool hasOrders = (await _orderItemRepo.FindAsync(oi => oi.ProductId == product.Id)).Any();
                //if (hasOrders)
                //    return new ApiResponse((int)HttpStatusCode.Conflict, "Cannot delete product. It has existing orders.", null, false);

                _productRepo.Remove(product);
                await _productRepo.SaveChangesAsync();
                return new ApiResponse((int)HttpStatusCode.OK, "Product Deleted Successfully", null, true);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while Deleting Order with Id {id}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> GetAllProductsAsync()
        {
            try
            {
                var product = await _productRepo.GetAllAsync();
                if (!product.Any())
                    return new ApiResponse((int)HttpStatusCode.NotFound, "No Data Found", null, false);
                var formatedProduct = product.Select(p => MapProductResponse(p));
                return new ApiResponse((int)HttpStatusCode.OK, "Product Retrieve Successfully", formatedProduct, true);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while Retreiving All product ");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(i => i.Id == id);
                if (product == null)
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Product Not Found", null, false);
                var formatedProduct = MapProductResponse(product);
                return new ApiResponse((int)HttpStatusCode.OK, "Product Retrieve Successfully", formatedProduct, true);

            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while Retrieve product with Id {id}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> UpdateProductAsync(ProductDTO.UpdateProductDTO dto,Guid id)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(i => i.Id == dto.Id);
                if (product == null)
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Product Not Found", null, false);
                product.Name = dto.Name;
                product.StockQuantity = dto.StockQuantity;
                product.Price = dto.Price;
                product.Description = dto.Description;
                product.UpdatedAt = DateTime.UtcNow;
                _productRepo.Update(product);
                await _productRepo.SaveChangesAsync();
                return new ApiResponse((int)HttpStatusCode.OK, "Product Update Successfully", product, true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while Updating product with name {dto.Id}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        private ProductResponseDTO MapProductResponse(Product data)
        {
            return new ProductResponseDTO
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                StockQuantity = data.StockQuantity,
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
            };
        }
    }
}
