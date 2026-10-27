using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace IzinYonetimSistemi.LeaveTypes.Dto
{
    [AutoMapTo(typeof(Entities.LeaveType))]
    public class CreateLeaveTypeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? MaxDaysPerYear { get; set; }

        public bool RequiresDocument { get; set; }

    }
}