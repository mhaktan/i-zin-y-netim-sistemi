using System;
using Abp.Application.Services.Dto;

namespace IzinYonetimSistemi.LeaveRequests.Dto
{
    public class PagedLeaveRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? EmployeeId { get; set; }
        public long? LeaveTypeId { get; set; }
        public string RequestNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TotalDays { get; set; }
        public string Reason { get; set; }
        public int? Status { get; set; }
        public string RevisionNote { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public DateTime? EndDateFrom { get; set; }
        public DateTime? EndDateTo { get; set; }
        public int? TotalDaysFrom { get; set; }
        public int? TotalDaysTo { get; set; }
        /// <summary>Virgülle ayrılmış enum indeksleri — ör. "0,2"</summary>
        public string StatusIn { get; set; }
        public int? StatusNot { get; set; }
    }
}
