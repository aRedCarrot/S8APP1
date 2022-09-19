using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CoupDeSonde.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
