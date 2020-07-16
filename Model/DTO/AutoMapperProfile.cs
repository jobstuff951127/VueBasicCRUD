using AutoMapper;

namespace Model.DTO
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ContactosMavidavidBlancos, ContactosMavidavidBlancosDTO>();
            CreateMap<ContactosMavidavidBlancosDTO, ContactosMavidavidBlancos>();
            CreateMap<ContactUpdateDto, ContactosMavidavidBlancos>();
        }
    }
}