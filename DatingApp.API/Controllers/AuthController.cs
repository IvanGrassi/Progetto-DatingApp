using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Data_Transfer_Objects;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    //determiniamo la route per usare il controller
    [Route("api/[controller]")] //Controller viene sostituito con Auth(controller)
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        //iniettiamo la nuova repository
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }


        //http://localhost:5000/api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //string e username li riceviamo come unico oggetto (tutto insieme)
            //validiamo la richiesta

            //convertiamo lo username in lowercase
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            //creazione dello user
            if (await _repo.UserExists(userForRegisterDto.Username))
            {
                return BadRequest("Username already exists");
            }

            //creazione di una nuova istanza della classe user
            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            //verrà inviato lo username creato e la password collegata ad esso
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201); //ritorno (per ora) una richiesta di "found" e quindi creato correttamente
        }


        //http://localhost:5000/api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            //riceve lo username e la password inseriti
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)    //non permette valori nulli
            {
                return Unauthorized();
            }

            //qui sono contenute le richieste contenute nel token per poter accedere ai dati presenti nel db
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            //chiave per tracciare il token (verrà sottoposta ad hash e non leggibile all'interno del token)
            var key = new SymmetricSecurityKey(Encoding.UTF8.
            GetBytes(_config.GetSection("AppSettings:Token").Value));

            //salvataggio delle credenziali che vengono crittografate (per verificare che il token sia valido)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        
            //qui creiamo effettivamente il token a cui passiamo i claims esettiamo la scadenza del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),   //si basa sulla richiesta
                Expires = DateTime.Now.AddDays(1),      //e scade dopo un giorno
                SigningCredentials = creds
            };

            //gestione del token tramite JWT
            var tokenHandler = new JwtSecurityTokenHandler();

            //creazione token JWT in base al tokenDescriptor e passaggio al token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //usiamo la variabile token per scrivere il token come risposta da mandare al client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });


            //token testabile su postman (vedi httpPost) e copiando incollando il codice criptato in jwt.io
        }
    }
}