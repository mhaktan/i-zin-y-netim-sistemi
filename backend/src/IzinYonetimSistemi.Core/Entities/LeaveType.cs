using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IzinYonetimSistemi.Entities
{
    [Table("LeaveTypes")]
    public class LeaveType : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int? MaxDaysPerYear { get; set; }

        public bool RequiresDocument { get; set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }

    }
}