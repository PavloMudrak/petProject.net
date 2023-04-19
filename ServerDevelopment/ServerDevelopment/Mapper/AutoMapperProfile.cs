using AutoMapper;
using DataAccessLayer.Models;
using ServerDevelopment.Data;

namespace ServerDevelopment.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailAddress))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));
        }
    }
}
