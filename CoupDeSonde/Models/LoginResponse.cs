namespace CoupDeSonde.Models
{
    public class LoginResponse
    {
        public string Username { get; set; }
        public string Token { get; set; }


        public LoginResponse(User user, string token)
        {
            Username = user.Username;
            Token = token;
        }
    }
}
