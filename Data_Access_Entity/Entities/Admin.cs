using System.ComponentModel.DataAnnotations;

namespace Data_Access_Entity.Entities
{
    public class Admin
    {
        public int ID { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

}