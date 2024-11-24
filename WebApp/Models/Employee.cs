using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
