using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }          
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }    // se la foto é la foto principale


        // aggiungendo questi si può ottenere un cascade delete
        // se lo user viene eliminato, anche le foto collegate a lui vengono eliminate
        public User User { get; set; }
        public int UserId { get; set; }
    }
}