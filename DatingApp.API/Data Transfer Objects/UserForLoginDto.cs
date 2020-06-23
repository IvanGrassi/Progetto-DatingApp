namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForLoginDto
    {
        //gestisce i parametri usati per effettuare il login
        public string Username { get; set; }
        public string Password { get; set; }
    }
}