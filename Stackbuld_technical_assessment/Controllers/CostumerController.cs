using Core.DTOs;
using Core.Interface;
using Infrastracture.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Core.DTOs.CostumerDTO;
using static Core.DTOs.ProductDTO;

namespace Api.Controllers
{
    [ApiController]
    public class CostumerController : RootController
    {
        private readonly ICostumerRepo _costumerRepo;
        public CostumerController(ICostumerRepo costumerRepo)
        {
            _costumerRepo = costumerRepo;
        }
        [HttpPost("Createcostumer")]
        public async Task<IActionResult> CreateCostumer([FromBody] CreateCostumerRequest dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _costumerRepo.CreateCostumerAsync(dto);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }
            var product = response.Data as Costumer;
            return CreatedAtAction(nameof(GetCostumerById), new { id = product?.Id }, response);
        }
        [HttpDelete("DeleteCostumer/{id}")]
        public async Task<IActionResult> DeleteCostumer(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _costumerRepo.DeleteCostumerAsync(id);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
        [HttpGet("GetCostumerById/{id}")]
        public async Task<IActionResult> GetCostumerById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _costumerRepo.GetAllCostumersAsync();

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllCostumer")]
        public async Task<IActionResult> GetAllCostumer()
        {


            var response = await _costumerRepo.GetAllCostumersAsync();

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }

        [HttpPut("UpdateCostumer/{id}")]
        public async Task<IActionResult> UpdateProduct(UpdateCostumerRequest dto, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _costumerRepo.UpdateCostumerAsync(dto, id);

            if (!response.Success)
            {
                // Send the exact status code provided by the service
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
    }
}
