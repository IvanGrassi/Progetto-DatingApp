using System.Linq;
using AutoMapper;
using DatingApp.API.Data_Transfer_Objects;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        // automapper usa i profiles per capire la source e la destination di ciò che viene mappato
        public AutoMapperProfiles()
        {
            // sorgente e destinazione mapping
            CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                src.Photos.FirstOrDefault(p => p.IsMain).Url))                  // configurazione di PhotoUrl
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>          
                src.DateOfBirth.CalculateAge()))                            // configurazione dell'età (calcolo: vedi in Extensions.cs)
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>          
                src.DateOfBirth.CalculateAge()));  

            CreateMap<User, UserForDetailedDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                src.Photos.FirstOrDefault(p => p.IsMain).Url))     // configurazione di PhotoUrl;
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>          
                src.DateOfBirth.CalculateAge()));  

            CreateMap<Photo, PhotosForDetailedDto>();

            CreateMap<UserForUpdateDto, User>(); // mappa le proprietà per l'update presenti in UserForUpdateDto.cs
        
            CreateMap<Photo, PhotoForReturnDto>(); // mappa le proprietà per ritornare la foto presenti in PhotoFroReturnDto.cs
        
            CreateMap<PhotoForCreationDto, Photo>(); // mappa tutte le principali proprietà delle foto
        }
    }
}