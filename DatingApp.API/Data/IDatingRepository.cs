using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
         // metodo per aggiungere
        void Add<T>(T entity) where T: class;  // aggiungi un tipo T (che sarebb user) dove T é una classe (la classe User)

         // metodo per eliminare
        void Delete<T>(T entity) where T: class; 
         // metodo per salvare le modifiche
        Task<bool> SaveAll();   // verifica se c'é stato piü di un salvataggio nel database, se non c'é ritorna true
         // metodo per ottenere tutti gli user
        Task<PagedList<User>> GetUsers(UserParams userParams);
         // metodo per ottenere un singolo user passando gli userParams
         Task<User> GetUser(int id);
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);
    }
}