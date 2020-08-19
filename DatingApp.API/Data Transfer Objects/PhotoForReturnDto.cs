using System;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class PhotoForReturnDto
    {
        // qui viene ritornata effettivamente la foto e non i dettagli

        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }          
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }    // se la foto é la foto principale
        public string PublicId { get; set; } // viene ritornato da cloudinary
    } 
}