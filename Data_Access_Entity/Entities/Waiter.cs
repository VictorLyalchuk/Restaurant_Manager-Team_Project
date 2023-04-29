using System.ComponentModel.DataAnnotations;

namespace Data_Access_Entity.Entities
{
    [Serializable]
    public class Waiter
    {
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SurName { get; set; }
        public double Salary { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartWorkingDate { get; set; }
        public ICollection<Table> Tables { get; set; }
        public ICollection<Order> Orders { get; set; }
        public int Raiting { get; set; }
    }
}