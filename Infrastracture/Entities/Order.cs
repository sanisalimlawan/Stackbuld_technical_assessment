using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Entities
{
    public class Order : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CosumerId { get; set; }
        public Costumer Costumer { get; set; }
        public ICollection<OrderItem> orderItems { get; set; } = new List<OrderItem>();
    }
}
