using System.ComponentModel.DataAnnotations;

namespace Data_Access_Entity.Entities
{
    [Serializable]
    public class Order
    {
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<ProductOrder>? ProductsOrders { get; set; }
        public int WaiterId { get; set; }
        public Waiter Waiter { get; set; }
        public bool Active { get; set; }
        public int TableId { get; set; }
        public double TotalSum { get; set; }
    }
}