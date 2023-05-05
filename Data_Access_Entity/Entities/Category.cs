using System.ComponentModel.DataAnnotations;

namespace Data_Access_Entity.Entities
{
    [Serializable]
    public class Category
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
}