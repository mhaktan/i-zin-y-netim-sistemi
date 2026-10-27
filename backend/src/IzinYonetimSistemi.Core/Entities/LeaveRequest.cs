using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace IzinYonetimSistemi.Entities
{
    // State Machine: status — Draft → PendingHRApproval → PendingManagerApproval → PendingHaktanApproval → Approved → Cancelled
    // Initial: Draft | Transitions: Draft→PendingHRApproval[Submit], PendingHRApproval→PendingManagerApproval[Approve], PendingHRApproval→Draft[Reject], PendingManagerApproval→PendingHaktanApproval[Approve], PendingManagerApproval→Draft[Reject], PendingHaktanApproval→Approved[Approve], PendingHaktanApproval→Draft[Reject], *→Cancelled[Cancel]
    [Table("LeaveRequests")]
    public class LeaveRequest : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string RequestNo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalDays { get; set; }

        [MaxLength(1000)]
        public string Reason { get; set; }

        public LeaveRequestStatus Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public long LeaveTypeId { get; set; }

        [ForeignKey(nameof(LeaveTypeId))]
        public virtual LeaveType LeaveType { get; set; }

    }
}