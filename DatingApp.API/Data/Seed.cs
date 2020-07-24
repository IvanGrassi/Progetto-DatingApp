using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        // in questa classe verranno usati i metodi che permetteranno di 
        // recuperare i dati dal file UserSeedData.json per poi serializzarli
        // nella classe User

        public static void SeedUsers(DataContext context){
            if(!context.Users.Any()) // nessun utente nel db
            {
                // file da leggere
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                // lista di utenti deserializzati
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                // a ogni utente viene assegnato un hash e un salt della pw
                foreach (var user in users)
                {
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);
                
                    // aggiunta passwordhash e salt alla password dello user
                    user.PasswordHash = passwordhash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //viene generato tramite HMACSHA512 il salt e l'hash
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;    //key generata casualmente
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); //compie l'hash relativo al byte [] passwordHash
            }
        }
    }
}