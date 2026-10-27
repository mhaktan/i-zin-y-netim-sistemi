using AutoMapper;
using IzinYonetimSistemi.Approvals.Dto;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.Approvals
{
    public class ApprovalMapProfile : Profile
    {
        public ApprovalMapProfile()
        {
            CreateMap<ApprovalRecord, ApprovalRecordDto>();
            CreateMap<StatusChangeLog, StatusChangeLogDto>();
        }
    }
}
