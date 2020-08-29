using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForRegisterDto
    {
        //le data annotation required mi permettono di non inserire un campo vuoto
        // queste proprietà dovranno essere mappate: AutoMapperProfiles.cs
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="You must specify password between 4 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string City { get; set; }
        
        [Required]
        public string Country { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

        public UserForRegisterDto()
        {
            //valori di default che si riferiscono al tempo attuale
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}