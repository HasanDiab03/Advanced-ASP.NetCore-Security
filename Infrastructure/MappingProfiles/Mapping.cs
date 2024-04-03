using AutoMapper;
using Common.Requests.Identity;
using Common.Responses.Identity;
using Infrastructure.Models;

namespace Infrastructure.MappingProfiles
{
	public class Mapping : Profile
	{
        public Mapping()
        {
            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<UpdateUserRequest, ApplicationUser>();
            CreateMap<ApplicationRole, RoleResponse>();
            CreateMap<ApplicationRoleClaim, RoleClaimViewModel>().ReverseMap();
        }
    }
}
