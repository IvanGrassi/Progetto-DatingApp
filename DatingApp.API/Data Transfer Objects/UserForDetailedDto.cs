using System;
using System.Collections.Generic;
using DatingApp.API.Models;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForDetailedDto
    {
        // qui verranno aggiunte le proprietà che verranno ritornate tutte le informazioni relative all'utente quando viene fatta una richiesta
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; } // url della foto principale dello user
        public ICollection<PhotosForDetailedDto> Photos { get; set; } // collezione di tutte le foto dello user
    }
}