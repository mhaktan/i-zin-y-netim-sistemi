using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace IzinYonetimSistemi.Employees.Dto
{
    [AutoMapTo(typeof(Entities.Employee))]
    public class CreateEmployeeDto
    {
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmployeeCode { get; set; }

        [MaxLength(100)]
        public string Department { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public DateTime? HireDate { get; set; }

        public int? AnnualLeaveBalance { get; set; }

        public long? UserId { get; set; }

    }
}