using Core.DTOs;
using Core.Interface;
using Infrastracture.Entities;
using Infrastracture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Core.DTOs.ProductDTO;

namespace Api.Controllers
{
    [ApiController]
    public class ProductController : RootController
    {
        private readonly IProductRepo _productRepo;
        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductReuest dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _productRepo.CreateProductAsync(dto);
            
            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }
            var product = response.Data as ProductResponseDTO;
            return CreatedAtAction(nameof(GetProductById), new { id = product?.Id }, response);
        }
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _productRepo.DeleteProductAsync(id);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _productRepo.GetProductByIdAsync(id);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {


            var response = await _productRepo.GetAllProductsAsync();

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(UpdateProductDTO dto,Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _productRepo.UpdateProductAsync(dto,id);

            if (!response.Success)
            {
                // Send the exact status code provided by the service
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
    }
}
