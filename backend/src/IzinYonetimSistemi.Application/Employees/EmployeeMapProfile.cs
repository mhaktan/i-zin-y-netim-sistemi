using AutoMapper;
using IzinYonetimSistemi.Entities;
using IzinYonetimSistemi.Employees.Dto;

namespace IzinYonetimSistemi.Employees
{
    public class EmployeeMapProfile : Profile
    {
        public EmployeeMapProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(d => d.UserDisplay, o => o.MapFrom(s => s.User != null ? s.User.Name.ToString() : null));
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
