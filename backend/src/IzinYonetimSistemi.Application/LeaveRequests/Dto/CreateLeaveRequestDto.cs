using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace IzinYonetimSistemi.LeaveRequests.Dto
{
    [AutoMapTo(typeof(Entities.LeaveRequest))]
    public class CreateLeaveRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string RequestNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long EmployeeId { get; set; }

        public long LeaveTypeId { get; set; }

    }
}