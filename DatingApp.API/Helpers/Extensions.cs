using System;
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

        // per calcolare l'età degli users
        public static int CalculateAge(this DateTime theDateTime)   //theDateTime: user DoB
        {
            var age = DateTime.Today.Year - theDateTime.Year; // data odierna - data di nascita
            if(theDateTime.AddYears(age) > DateTime.Today)
            {
                // se non ha ancora compiuto gli anni: tolgo 1 anno
                age--;
            }
            return age;
        }
    }
}