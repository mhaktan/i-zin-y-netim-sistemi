namespace IzinYonetimSistemi.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";

        // LeaveType
        public const string LeaveType_Read = "LeaveType.Read";
        public const string LeaveType_Create = "LeaveType.Create";
        public const string LeaveType_Update = "LeaveType.Update";
        public const string LeaveType_Delete = "LeaveType.Delete";

        // LeaveRequest
        public const string LeaveRequest_Read = "LeaveRequest.Read";
        public const string LeaveRequest_Create = "LeaveRequest.Create";
        public const string LeaveRequest_Update = "LeaveRequest.Update";
        public const string LeaveRequest_Delete = "LeaveRequest.Delete";
        public const string LeaveRequest_ChangeStatus = "LeaveRequest.ChangeStatus";

        // Employee
        public const string Employee_Read = "Employee.Read";
        public const string Employee_Create = "Employee.Create";
        public const string Employee_Update = "Employee.Update";
        public const string Employee_Delete = "Employee.Delete";

        // RBAC management
        public const string AppUser_Read = "AppUser.Read";
        public const string AppRole_Read = "AppRole.Read";
        public const string AppUser_Create = "AppUser.Create";
        public const string AppRole_Create = "AppRole.Create";
        public const string AppUser_Update = "AppUser.Update";
        public const string AppRole_Update = "AppRole.Update";
        public const string AppUser_Delete = "AppUser.Delete";
        public const string AppRole_Delete = "AppRole.Delete";
        public const string AppRole_AssignPermissions = "AppRole.AssignPermissions";

    }
}
