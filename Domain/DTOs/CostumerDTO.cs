using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CostumerDTO
    {
        public class CostumerResponse
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }
        public class UpdateCostumerRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }
        public class CreateCostumerRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
        }
    }
}
