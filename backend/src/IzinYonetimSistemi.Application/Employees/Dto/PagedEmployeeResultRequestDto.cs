using System;
using Abp.Application.Services.Dto;

namespace IzinYonetimSistemi.Employees.Dto
{
    public class PagedEmployeeResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? UserId { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Title { get; set; }
        public DateTime? HireDate { get; set; }
        public int? AnnualLeaveBalance { get; set; }
        public DateTime? HireDateFrom { get; set; }
        public DateTime? HireDateTo { get; set; }
        public int? AnnualLeaveBalanceFrom { get; set; }
        public int? AnnualLeaveBalanceTo { get; set; }
    }
}
