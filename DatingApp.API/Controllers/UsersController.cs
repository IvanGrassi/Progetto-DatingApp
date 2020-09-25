using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data_Transfer_Objects;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] // permette di applicare il log ad ogni metodo richiamato dallo user
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
        // FromQuery perché il sistema non realizza che vogliamo inviare i params come query
        // quindi il sistema non ha idea da dove arrivino gli userParams
        // FromQuery permette di inviare una querystring vuota
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // recupera l'id dello user

            var userFromRepo = await _repo.GetUser(currentUserId);  // recupera le informazioni legato a quell'id

            userParams.UserId = currentUserId;    // assegna lo il parametro UserId all'id corrente       

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                // se il gender = male, allora ritornerà female
                // se il gendere = female, allora ritornerà male
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams); // utenti richiamati tramite metodo getusers e contenuti nella variabile
            
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            
            // queste informazioni per la paginazione vengono passate dall'header al client
            Response.AddPagination(users.CurrentPage, users.PageSize,
             users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);   // risposta con codice 200 (ok) e ritorna la lista di users
        }

        [HttpGet("{id}", Name = "GetUser")] // Name = GetUser verrà inviato all'authController
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
                return Unauthorized(); // se non corrisponde: messaggio non autorizzato! (Unauthorized request)
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