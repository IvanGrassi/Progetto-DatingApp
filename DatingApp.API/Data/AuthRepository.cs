using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        //implementiamo tutti i metodi contenuti nell'interfaccia

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            // ritorna anche la collection di foto che servirà per visualizzare la foto in Welcome user
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);  //lo user viene salvato in una variabile, verifica se lo username é uguale a quello presente nel db

            if(user == null)
            {
                return null;
            }

            //comparazione delle password: se la password non corrisponde ritorna nullo

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }
            return user;
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //creiamo una nuova istanza di HMACSHA512, ma gli passiamo la key per il ComputeHash
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //compie l'hash della password usando la key contenuta in passwordSalt
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //compie l'hash relativo al byte [] passwordHash

                //compara ogni numero dell'hash, se corrispondono i due (computedHash e passwordHash), allora la password é corretta
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        //registrazione e creazione utente nel db
        public async Task<User> Register(User user, string password)
        {
            byte [] passwordHash, passwordSalt;
            //passiamo il riferimento alle due variabili hash e salt in modo che quando verranno aggiornate, avverrà anche qui
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _context.Users.AddAsync(user);    //aggiungiamo l'utente nel db
            await _context.SaveChangesAsync();      //salviamo l'update nel db

            return user;                            
        }

        //crittografare la password (Hash Message Authentication Mode)
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //viene generato tramite HMACSHA512 il salt e l'hash
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;    //key generata casualmente
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //compie l'hash relativo al byte [] passwordHash
            }
        }

        //verifica l'esistenza dell'user nel database
        public async Task<bool> UserExists(string username)
        {
            //se lo username inserito é = a quello presente in db
            if (await _context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }
            return false;
        }
    }
}