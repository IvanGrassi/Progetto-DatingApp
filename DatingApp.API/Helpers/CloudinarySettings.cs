namespace DatingApp.API.Helpers
{
    public class CloudinarySettings
    {
        // gestisce le proprietà di cloudinary (le vedo connettendomi a cloudinary sul web)
        // Attenzione: ricordarsi di aggiungere il servizio in startup.cs

        public string CloudName { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}