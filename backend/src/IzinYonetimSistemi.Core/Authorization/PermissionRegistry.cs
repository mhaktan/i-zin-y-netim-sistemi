using System.Collections.Generic;
using Abp.Dependency;

namespace IzinYonetimSistemi.Authorization
{
    /// <summary>Single permission descriptor — name, group (entity), description.</summary>
    public class PermissionInfo
    {
        public string Name { get; }
        public string Group { get; }
        public string Description { get; }
        public bool IsRbac { get; }

        public PermissionInfo(string name, string group, string description, bool isRbac)
        {
            Name = name; Group = group; Description = description; IsRbac = isRbac;
        }
    }

    public interface IPermissionRegistry
    {
        IReadOnlyList<PermissionInfo> All { get; }
    }

    public class PermissionRegistry : IPermissionRegistry, ISingletonDependency
    {
        public IReadOnlyList<PermissionInfo> All { get; } = new List<PermissionInfo>
        {
            new PermissionInfo("LeaveType.Read", "LeaveType", "Read LeaveType", false),
            new PermissionInfo("LeaveType.Create", "LeaveType", "Create LeaveType", false),
            new PermissionInfo("LeaveType.Update", "LeaveType", "Update LeaveType", false),
            new PermissionInfo("LeaveType.Delete", "LeaveType", "Delete LeaveType", false),
            new PermissionInfo("LeaveRequest.Read", "LeaveRequest", "Read LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Create", "LeaveRequest", "Create LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Update", "LeaveRequest", "Update LeaveRequest", false),
            new PermissionInfo("LeaveRequest.Delete", "LeaveRequest", "Delete LeaveRequest", false),
            new PermissionInfo("LeaveRequest.ChangeStatus", "LeaveRequest", "Change LeaveRequest status", false),
            new PermissionInfo("Employee.Read", "Employee", "Read Employee", false),
            new PermissionInfo("Employee.Create", "Employee", "Create Employee", false),
            new PermissionInfo("Employee.Update", "Employee", "Update Employee", false),
            new PermissionInfo("Employee.Delete", "Employee", "Delete Employee", false),
            new PermissionInfo("AppUser.Read", "AppUser", "Read users", true),
            new PermissionInfo("AppRole.Read", "AppRole", "Read roles", true),
            new PermissionInfo("AppUser.Create", "AppUser", "Create users", true),
            new PermissionInfo("AppRole.Create", "AppRole", "Create roles", true),
            new PermissionInfo("AppUser.Update", "AppUser", "Update users", true),
            new PermissionInfo("AppRole.Update", "AppRole", "Update roles", true),
            new PermissionInfo("AppUser.Delete", "AppUser", "Delete users", true),
            new PermissionInfo("AppRole.Delete", "AppRole", "Delete roles", true),
            new PermissionInfo("AppRole.AssignPermissions", "AppRole", "Assign permissions to roles", true),
        };
    }
}
