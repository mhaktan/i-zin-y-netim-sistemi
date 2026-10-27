namespace IzinYonetimSistemi.Entities
{
    public enum LeaveRequestStatus
    {
        Draft = 0,
        PendingHRApproval = 1,
        PendingManagerApproval = 2,
        PendingHaktanApproval = 3,
        Approved = 4,
        Cancelled = 5,
    }
}