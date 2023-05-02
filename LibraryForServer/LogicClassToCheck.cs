using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForServer
{
    [Serializable]
    public class LogicClassToCheck : LogicClass
    {
        public int RecipientId { get; set; }
        public int OrderId { get; set; }
        public int TableID { get; set; }
        public Order Order { get; set; }
    }
}
