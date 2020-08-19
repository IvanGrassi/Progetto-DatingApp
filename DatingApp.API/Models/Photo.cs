using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        // proprietà presenti in ogni foto
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }          
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }    // se la foto é la foto principale
        public string PublicId { get; set; } // serve all API per salvare nel database il PhotoUrl e il suo publicId

        // aggiungendo questi si può ottenere un cascade delete
        // se lo user viene eliminato, anche le foto collegate a lui vengono eliminate
        public User User { get; set; }
        public int UserId { get; set; }
    }
}