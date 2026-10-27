using AutoMapper;
using IzinYonetimSistemi.Entities;
using IzinYonetimSistemi.LeaveRequests.Dto;

namespace IzinYonetimSistemi.LeaveRequests
{
    public class LeaveRequestMapProfile : Profile
    {
        public LeaveRequestMapProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.EmployeeDisplay, o => o.MapFrom(s => s.Employee != null ? s.Employee.FullName.ToString() : null))
                .ForMember(d => d.LeaveTypeDisplay, o => o.MapFrom(s => s.LeaveType != null ? s.LeaveType.Name.ToString() : null));
            CreateMap<CreateLeaveRequestDto, LeaveRequest>();
            CreateMap<LeaveRequestDto, LeaveRequest>();
        }
    }
}
