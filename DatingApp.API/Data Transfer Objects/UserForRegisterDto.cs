using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class UserForRegisterDto
    {
        //le data annotation required mi permettono di non inserire un campo vuoto
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="You must specify password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}