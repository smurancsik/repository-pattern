using AutoMapper;
using Pegazus.Logic.Models;

namespace Pegazus.API.Dto.Profiles
{
    public class CustomerDtoProfile: Profile
    {
        public CustomerDtoProfile()
        {
            CreateMap<CustomerModel, CustomerDto>();
        }
    }
}
