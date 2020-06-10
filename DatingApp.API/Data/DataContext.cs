using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}
    
        public DbSet<Value> Values { get; set; }    //qui specifichiamo il nome delle tabelle che verranno create quando viene usato il comando scaffold
    }
}