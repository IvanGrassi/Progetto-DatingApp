using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
         //contiene 3 metodi
         //1. Registrazione dell'utente
         //2. Per effettuare il login all'API
         //3. Verifica se l'utente é già esistente o non esiste
    
        Task<User> Register (User user, string password);
        Task<User> Login (string username, string password);
        Task<bool> UserExists (string username);
    }
}