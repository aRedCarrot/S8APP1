using System.Text.Json.Serialization;

namespace CoupDeSonde.Models
{
    public class User
    {
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }
}
