using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForServer
{
    [Serializable]
    public class LogicClassToMessages : LogicClass
    {
        public string Message { get; set; }
    }
}
