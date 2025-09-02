using Core.DTOs;
using Core.Interface;
using Infrastracture.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Core.DTOs.OrderDTO;
using static Core.DTOs.ProductDTO;

namespace Api.Controllers
{
    [ApiController]
    public class OrderController : RootController
    {
        private readonly IOrderRepo _orderRepo;
        public OrderController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _orderRepo.PlaceOrder(dto);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }
            var order = response.Data as OrderResponse;
            return CreatedAtAction(nameof(GetOrderById), new { id = order?.OrderId }, response);
        }
        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(
                    (int)HttpStatusCode.BadRequest,
                    "Invalid data request",
                    ModelState,
                    false));
            }

            var response = await _orderRepo.GetOrdersById(id);

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
        [HttpGet("GetAllOders")]
        public async Task<IActionResult> GetAllProduct()
        {


            var response = await _orderRepo.GetAllOrders();

            if (!response.Success)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }

    }
}
