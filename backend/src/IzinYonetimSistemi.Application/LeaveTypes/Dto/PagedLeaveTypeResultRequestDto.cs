using System;
using Abp.Application.Services.Dto;

namespace IzinYonetimSistemi.LeaveTypes.Dto
{
    public class PagedLeaveTypeResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? MaxDaysPerYear { get; set; }
        public bool? RequiresDocument { get; set; }
        public int? MaxDaysPerYearFrom { get; set; }
        public int? MaxDaysPerYearTo { get; set; }
    }
}
