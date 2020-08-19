using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class PhotoForCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; } // rappresenta la foto (file inviato con richiesta http)
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }    // viene ritornato da cloudinary

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}