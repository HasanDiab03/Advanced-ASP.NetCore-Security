using AutoMapper;
using Common.Requests;
using Domain;

namespace Application.MappingProfiles
{
	public class Mapper : Profile
	{
        public Mapper()
        {
            CreateMap<UpdateEmployeeRequest, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<CreateEmployeeRequest, Employee>();
            CreateMap<Employee, Employee>(); // For Update
        }
    }
}
