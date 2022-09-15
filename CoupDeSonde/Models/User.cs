using System.Text.Json.Serialization;

namespace CoupDeSonde.Models
{
    public class User
    {
        public string Username { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

    }
}
