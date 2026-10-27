using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace IzinYonetimSistemi.Employees.Dto
{
    [AutoMapFrom(typeof(Entities.Employee))]
    public class EmployeeDto : EntityDto<long>
    {
        public string FullName { get; set; }

        public string EmployeeCode { get; set; }

        public string Department { get; set; }

        public string Title { get; set; }

        public DateTime? HireDate { get; set; }

        public int? AnnualLeaveBalance { get; set; }

        public long? UserId { get; set; }

        public string UserDisplay { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}