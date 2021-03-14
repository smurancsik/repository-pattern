using AutoMapper;
using Pegazus.Domain.Models;

namespace Pegazus.Logic.Models.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>()
                .ForMember(dst => dst.FirstName, options => options.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.LastName, options => options.MapFrom(src => src.LastName))
                .ForMember(dst => dst.Title, options => options.MapFrom(src => src.Title));
        }
    }
}
