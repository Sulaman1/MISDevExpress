using System.ComponentModel.DataAnnotations;

namespace BLEPMIS.Models
{
    public class ApiUser
    {
        [Key]
        public string Username { get; set; }

        public string Password { get; set; }
    }
}