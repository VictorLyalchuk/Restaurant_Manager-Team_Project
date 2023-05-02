using System.ComponentModel.DataAnnotations;

namespace Data_Access_Entity.Entities
{
    public class Product
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductOrder>? ProductsOrders { get; set; }
        public double Price { get; set; }
        public override string ToString()
        {
            return $"{Name} | {Price} $";
        }
    }
}