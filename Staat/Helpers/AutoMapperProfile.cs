using AutoMapper;
using Staat.Models.Users;

namespace Staat.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}