using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        //classe di estensione usabile per qualsiasi altro extension method
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // in caso di Exception quando inviamo al client, ci sarà un instestazione che mostra il messaggio di errore
            // le due intestazioni a seguire saranno le unniche ad essere mostrate

            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}