using System.Collections.Generic;
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
    }
}