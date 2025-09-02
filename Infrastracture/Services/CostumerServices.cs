using Core.DTOs;
using Core.Interface;
using Infrastracture.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Services
{
    public class CostumerServices : ICostumerRepo
    {
        private readonly ILogger<CostumerServices> _logger;
        private readonly IGenericRepo<Costumer> _costumerRepo;
        public CostumerServices(ILogger<CostumerServices> logger,IGenericRepo<Costumer> costumer)
        {
            _logger = logger;
            _costumerRepo = costumer;
        }
        public async Task<ApiResponse> CreateCostumerAsync(CostumerDTO.CreateCostumerRequest dto)
        {
            try
            {
                var costumer = new Costumer
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name,
                    Address = dto.Address,
                    Email = dto.Email,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await _costumerRepo.AddAsync(costumer);
                await _costumerRepo.SaveChangesAsync();
                return new ApiResponse((int)HttpStatusCode.Created, "Costumer Added Sucessfully", costumer, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while creating product with name {dto.Name}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> DeleteCostumerAsync(Guid id)
        {
            try
            {
                var costumer = await _costumerRepo.GetByIdAsync(x => x.Id == id);
                if(costumer == null)
                {
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Costumer Not Found", null, false);
                }
                _costumerRepo.Remove(costumer);
                await _costumerRepo.SaveChangesAsync();
                return new ApiResponse((int)HttpStatusCode.OK, "Costumer Added Sucessfully", costumer, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Deleting Costumer with Id {id}");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> GetAllCostumersAsync()
        {
            try
            {
                var costumer = await _costumerRepo.GetAllAsync();
                if (costumer == null || !costumer.Any())
                {
                    return new ApiResponse((int)HttpStatusCode.NotFound, "No Data Found", null, false);
                }
                return new ApiResponse((int)HttpStatusCode.NotFound, "Costumers Retrieve successfully", costumer, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Retrieving Costumers ");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }

        public async Task<ApiResponse> UpdateCostumerAsync(CostumerDTO.UpdateCostumerRequest dto,Guid id)
        {
            try
            {
                var costumer = await _costumerRepo.GetByIdAsync(i => i.Id == id);
                if (costumer == null)
                {
                    return new ApiResponse((int)HttpStatusCode.NotFound, "Costumer Not Found", null, false);
                }
                costumer.Name = dto.Name;
                costumer.Email = dto.Email;
                costumer.Address = dto.Address;
                costumer.UpdatedAt = DateTime.UtcNow;
                _costumerRepo.Update(costumer);
                await _costumerRepo.SaveChangesAsync();

                return new ApiResponse((int)HttpStatusCode.OK, "Costumers Updated successfully", costumer, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while Retrieving Costumers ");
                return new ApiResponse((int)HttpStatusCode.InternalServerError, "An UnExpected Error Occur please try again Letter", null, false);
            }
        }
    }
}
