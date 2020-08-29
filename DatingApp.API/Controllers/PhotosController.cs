using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.Data_Transfer_Objects;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]     // valida le richieste in modo automatico
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, 
        IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            // creo una nuova istanza di cloudinary dove verranno passati i dati dell'account
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name ="GetPhoto")] // nome della route
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id); // recupera la foto tramite GetPhoto creato in DatingRepository.cs
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, 
        [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            // viene comparato lo user id con il parametro user id presente nella route in cima a questa classe
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized(); // se non corrisponde: messaggio non autorizzato! (Unauthorized request)
            }
            // i dati contenuti in userForUpdateDto vengono mappati in userFromRepo
            // mapping: AutoMapperProfiles.cs
            
            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDto.File;
            var uploadResult = new ImageUploadResult(); // contiene i dati ritornati da cloudinary
        
            // verichiamo la presenza di dati nel file
            if(file.Length > 0)
            {
                // creiamo un nuovo FileStrea e leggiamo il file dell'immagine
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        //se l'immagine é troppo grossa, verrà ridotta in automatico
                        Transformation = new Transformation()
                        .Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    // passiamo i dati per l'upload in cloudinary e carichiamo richiamando il metodo Upload
                    uploadResult = _cloudinary.Upload(uploadParams);    
                }
            }
            // popolazione dei dati presenti in photoForCreationDto aggiornandoli
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            // viene mappato il photoForCreationDto in Photo basandosi sulle proprietà
            var photo = _mapper.Map<Photo>(photoForCreationDto);
        
            // se é la prima foto che carica, diventa anche la foto principale del profilo
            if (!userFromRepo.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }

            userFromRepo.Photos.Add(photo);

            if(await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo); // la foto avrà un id generato da sqlite e contenuto nella variabile photoToReturn
                // Route name: GetPhoto (metodo presente in DatingRepository.cs)
                // Route value: 
                return CreatedAtRoute("GetPhoto", new {userId = userId, id = photo.Id},
                photoToReturn); // ritornato http state: 201
            }

            return BadRequest("Could not add the photo");
        }

        // percorso usato per settare la foto come principale
        // http://localhost:5000/api/users/2/photos/15/setMain

        [HttpPost("{id}/setMain")] 
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            // viene comparato lo user id con il parametro user id presente nella route in cima a questa classe
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized(); // se non corrisponde: messaggio non autorizzato! (Unauthorized request)
            }

            // verifica che l'id {id} sia equvalente all'id dello user collegato alla foto
            var user = await _repo.GetUser(userId);

            // se l'id che passiamo NON corrisponde con nessuna id della foto (tra tutte le foto dell'utente)
            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized(); // NON autorizzato
            }

            // recupero della foto dal repo
            var photoFromRepo = await _repo.GetPhoto(id);
            
            // SE la foto che vuole cambiare é uguale alla foto che é già applicata come principale
            if(photoFromRepo.IsMain)
            {
                return BadRequest("This is already the main photo");
            }
            
            // la foto attualmente come main non diventa più main
            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            // e la nuova foto che viene aggiunta, diventa la nuova main
            photoFromRepo.IsMain = true;

            // salviamo le modifiche
            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not set the photo to main");
        }


        // eliminazione di una delle foto caricate nel profilo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            // viene comparato lo user id con il parametro user id presente nella route in cima a questa classe
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized(); // se non corrisponde: messaggio non autorizzato! (Unauthorized request)
            }

            // verifica che l'id {id} sia equvalente all'id dello user collegato alla foto
            var user = await _repo.GetUser(userId);

            // se l'id che passiamo NON corrisponde con nessuna id della foto (tra tutte le foto dell'utente)
            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized(); // NON autorizzato
            }

            // recupero della foto dal repo
            var photoFromRepo = await _repo.GetPhoto(id);
            
            // SE la foto che vuole cambiare é uguale alla foto che é già applicata come principale
            if(photoFromRepo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }

            if(photoFromRepo.PublicId != null)
            {
                //documentazione cloudinary

                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                // se la risposta data come risultato dal server é OK, allora elimino la foto
                if(result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if(photoFromRepo.PublicId == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if(await _repo.SaveAll())
            { // modifiche salvate
                return Ok();
            }

            return BadRequest("Failed to delete the photo");
        }
    }
}