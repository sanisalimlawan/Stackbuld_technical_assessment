using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.DTOs.CostumerDTO;

namespace Core.Interface
{
    public interface ICostumerRepo
    {
        public Task<ApiResponse> CreateCostumerAsync(CreateCostumerRequest dto);
        public Task<ApiResponse> UpdateCostumerAsync(UpdateCostumerRequest dto,Guid id);
        public Task<ApiResponse> DeleteCostumerAsync(Guid id);
        public Task<ApiResponse> GetAllCostumersAsync();

    }
}
