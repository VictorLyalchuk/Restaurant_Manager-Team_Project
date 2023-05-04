using PropertyChanged;
using System.Xml.Linq;

namespace Data_Access_Entity.Entities
{
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class Table
    {
        public int ID { get; set; }
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        public bool Active { get; set; }
        public override string ToString()
        {
            return $"{ID}";
        }
    }
}