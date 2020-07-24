using System;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForListDto
    {
        // qui verranno aggiunte le proprietà che verranno ritornati tutti gli utenti quando viene fatta una richiesta
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; } // url della foto principale dello user
    }
}