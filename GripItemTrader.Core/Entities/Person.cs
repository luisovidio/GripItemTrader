using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GripItemTrader.Core.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
