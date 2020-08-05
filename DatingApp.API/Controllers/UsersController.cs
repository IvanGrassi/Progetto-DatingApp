using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data_Transfer_Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]     // autorizziamo tutti i metodi prima di ritornare dati
    [Route("api/[controller]")] //Controller viene sostituito con User(controller)
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers(); // utenti richiamati tramite metodo getusers e contenuti nella variabile
            
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            
            return Ok(usersToReturn);   // risposta con codice 200 (ok) e utenti inviati
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id); // come sopra ma per lo specifico user tramite id
            
            // qui automapper mapperà ogni proprietà facendo riferimento allo user.cs
            // tra le <> va aggiunta la destinazione del mapping
            // tra le () va aggiunta la sorgente
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]   // usato per aggiornare i dati in un api (tipico update)
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            // confronto id dello user che vuole fare la modifica con il token che il server riceve
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized(); // se non corrisponde: messaggio non autorizzato!
            }
            // i dati contenuti in userForUpdateDto vengono mappati in userFromRepo
            // mapping: AutoMapperProfiles.cs
            
            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);

            if(await _repo.SaveAll())
            {
                return NoContent(); //salva i dati mappati senza ritornare nulla
            }
            throw new System.Exception($"Updating user {id} failed on save");
        }
    }
}