using System.ComponentModel.DataAnnotations;

namespace firstapi.Model
{
    public class user
    {
        [Key]
        public int Id { get; set;}

        [Required]
        public string UserName { get; set;}

        [Required]
        public string Password { get; set; }
    }
}
