using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        // Implementa tutte le interfacce sviluppate in IdatingRepository
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
            .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            // viene passato l'id per estrarre il primo id che corrisponde a ciò che stiamo passando (u.Id == id)
            // altrimenti viene ritornato null come default
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // otteniamo gli users dal context e li ordiniamo in modo discendente
            var users = _context.Users.Include(p => p.Photos)
            .OrderByDescending(u => u.LastActive).AsQueryable();

            // ritorna i dati dello user dove l'id é = allo UserId (trovato nello UsersController)
            users = users.Where(u => u.Id != userParams.UserId);

            // ritorna i dati dello user dove il gender é = al Gender( trovato nello UsersController)
            users = users.Where(u => u.Gender == userParams.Gender);
            
            // verifica il range di età (se < 18 anni o > 99)
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                // data minima: oggi - 98 (non includo 99 perché andrei oltre il range)
                var minDateOfBirth = DateTime.Today.AddYears(-userParams.MaxAge -1);
                // data massima: oggi -18 
                var maxDateOfBirth = DateTime.Today.AddYears(-userParams.MinAge);

                // where dataMinima >= 18 e dataMassima <=99
                users = users.Where(u => u.DateOfBirth >= minDateOfBirth && u.DateOfBirth <= maxDateOfBirth);
            }

            // ORDINAMENTO
            if (string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    // se lo user params é settato su "created", gli users vengono ritornati
                    // in base alla loro data di creazione (di iscrizione)
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            // ritorna una pagedList di users creando la PagedList con CreateAsync
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            // se ritorna > 0, ottengo true
            // se é = 0, ottengo false, nulla é stato salvato nel database
            return await _context.SaveChangesAsync() > 0;
        }
    }
}