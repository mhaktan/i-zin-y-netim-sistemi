using AutoMapper;
using IzinYonetimSistemi.Entities;
using IzinYonetimSistemi.LeaveTypes.Dto;

namespace IzinYonetimSistemi.LeaveTypes
{
    public class LeaveTypeMapProfile : Profile
    {
        public LeaveTypeMapProfile()
        {
            CreateMap<LeaveType, LeaveTypeDto>();
            CreateMap<CreateLeaveTypeDto, LeaveType>();
            CreateMap<LeaveTypeDto, LeaveType>();
        }
    }
}
