using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.OrderDTO;

namespace Core.Interface
{
    public interface IOrderRepo
    {
        public Task<ApiResponse> PlaceOrder(PlaceOrderRequest dto);
        public Task<ApiResponse> GetAllOrders();
        public Task<ApiResponse> GetOrdersById(Guid id);
    }
}
