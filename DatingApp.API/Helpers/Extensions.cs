using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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

        public static void AddPagination(this HttpResponse response,
         int currentPage, int itemsPerPage, int totalItems, int totalPages)
         {
            // creazione nuova istanza di PaginationHeader
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings(); // permetterà di parsare i valori nel json in camelcase (Serve per angular)
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
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