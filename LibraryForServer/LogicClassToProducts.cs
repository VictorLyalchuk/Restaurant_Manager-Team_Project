using Data_Access_Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForServer
{
    [Serializable]
    public class LogicClassToProducts : LogicClass
    {
        public List<int> Products { get; set; }
        public int RecepietId { get; set; }
    }
}
