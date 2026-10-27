using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace IzinYonetimSistemi.LeaveTypes.Dto
{
    [AutoMapFrom(typeof(Entities.LeaveType))]
    public class LeaveTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? MaxDaysPerYear { get; set; }

        public bool RequiresDocument { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}