using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForServer
{
    [Serializable]
    public class LogicClassToMessage : LogicClass
    {
        public int RecipientId { get; set; }
        public string Message { get; set; }

    }
}
