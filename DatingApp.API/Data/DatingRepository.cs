using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        // Implementa l'interfaccia IDatingRepostory
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

        public async Task<User> GetUser(int id)
        {
            // viene passato l'id per estrarre il primo id che corrisponde a ciò che stiamo passando (u.Id == id)
            // altrimenti viene ritornato null come default
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            // vengono ritornati gli users sottoforma di lista
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // se ritorna > 0, ottengo true
            // se é = 0, ottengo false, nulla é stato salvato nel database
            return await _context.SaveChangesAsync() > 0;
        }
    }
}