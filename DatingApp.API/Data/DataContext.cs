using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
    
        //quando creiamo una nuova classe o modifichiamo una proprietà di una classe contenuta nel Model
        //dobbiamo aggiungere una nuova migration (vedi qua sotto)

        public DbSet<Value> Values { get; set; }    //qui specifichiamo il nome delle tabelle che verranno create quando viene usato il comando scaffold
    
        public DbSet<User> Users { get; set; }
    }
}