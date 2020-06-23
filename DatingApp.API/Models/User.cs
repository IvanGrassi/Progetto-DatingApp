namespace DatingApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; } //contiene l'hash della password
        public byte[] PasswordSalt { get; set; }
    }
}