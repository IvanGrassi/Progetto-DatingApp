using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // ogni volta che lo user richiama questo metodo, la proprietà lastActive vien aggiornata
        // ActionExecutingContext: quando l'azione viene eseguita
        // ActionExecutionDelegate: permette di eseguire codice dopo che l'azione é stata eseguita (o prima o durante)

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // permette l'accesso all'HTTPContext per le azioni eseguite
        
            // id recuperato dal token
            var userId = int.Parse(resultContext.HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier).Value);

            // otteniamo un istanza del servizio
            var repo = resultContext.HttpContext.RequestServices.GetService<IDatingRepository>();
        
            // recuperare lo user
            var user = await repo.GetUser(userId);

            // otteniamo cosi la lastActive date dello user
            user.LastActive = DateTime.Now;

            // salvataggio
            await repo.SaveAll();
        }
    }
}