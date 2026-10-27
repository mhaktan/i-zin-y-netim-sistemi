using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IzinYonetimSistemi.Entities
{
    [Table("Employees")]
    public class Employee : FullAuditedEntity<long>
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

        [ForeignKey(nameof(UserId))]
        public virtual AppUser User { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

    }
}