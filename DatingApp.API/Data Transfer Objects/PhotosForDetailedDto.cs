using System;

namespace DatingApp.API.Data_Transfer_Objects
{
    public class PhotosForDetailedDto
    {
        // vengono ritornati tutti i dettagli della foto (ma NON la foto)
        // di quello si occupa PhotoForReturnDto.cs

        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }          
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }    // se la foto é la foto principale
    }
}