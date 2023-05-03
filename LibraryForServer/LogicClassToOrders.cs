using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForServer
{
    [Serializable]
    public class LogicClassToOrders : LogicClass
    {
        public IEnumerable<ProductOrder> products { get; set; }
        public Order order { get; set; }
        public int RecepientId { get; set; }
        public string Msg { get; set; }

    }
}
