using Abp.Authorization;
using Abp.Localization;

namespace IzinYonetimSistemi.Authorization
{
    public class IzinYonetimSistemiAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull("Pages") ?? context.CreatePermission("Pages", L("Pages"));

            // LeaveType
            pages.CreateChildPermission(PermissionNames.LeaveType_Read, L("LeaveType.Read"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Create, L("LeaveType.Create"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Update, L("LeaveType.Update"));
            pages.CreateChildPermission(PermissionNames.LeaveType_Delete, L("LeaveType.Delete"));

            // LeaveRequest
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Read, L("LeaveRequest.Read"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Create, L("LeaveRequest.Create"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Update, L("LeaveRequest.Update"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_Delete, L("LeaveRequest.Delete"));
            pages.CreateChildPermission(PermissionNames.LeaveRequest_ChangeStatus, L("LeaveRequest.ChangeStatus"));

            // Employee
            pages.CreateChildPermission(PermissionNames.Employee_Read, L("Employee.Read"));
            pages.CreateChildPermission(PermissionNames.Employee_Create, L("Employee.Create"));
            pages.CreateChildPermission(PermissionNames.Employee_Update, L("Employee.Update"));
            pages.CreateChildPermission(PermissionNames.Employee_Delete, L("Employee.Delete"));

            // RBAC
            pages.CreateChildPermission(PermissionNames.AppUser_Read, L("AppUser.Read"));
            pages.CreateChildPermission(PermissionNames.AppRole_Read, L("AppRole.Read"));
            pages.CreateChildPermission(PermissionNames.AppUser_Create, L("AppUser.Create"));
            pages.CreateChildPermission(PermissionNames.AppRole_Create, L("AppRole.Create"));
            pages.CreateChildPermission(PermissionNames.AppUser_Update, L("AppUser.Update"));
            pages.CreateChildPermission(PermissionNames.AppRole_Update, L("AppRole.Update"));
            pages.CreateChildPermission(PermissionNames.AppUser_Delete, L("AppUser.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_Delete, L("AppRole.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_AssignPermissions, L("AppRole.AssignPermissions"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, IzinYonetimSistemiConsts.LocalizationSourceName);
        }
    }
}
