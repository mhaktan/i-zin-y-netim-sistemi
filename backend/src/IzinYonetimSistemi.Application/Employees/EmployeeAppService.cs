using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using IzinYonetimSistemi.Entities;
using IzinYonetimSistemi.Employees.Dto;
using IzinYonetimSistemi.Analytics.Dto;
using IzinYonetimSistemi.LeaveRequests.Dto;
using IzinYonetimSistemi.Approvals.Dto;
using IzinYonetimSistemi.Authorization;
using IzinYonetimSistemi.Flows;

namespace IzinYonetimSistemi.Employees
{
    [Abp.Authorization.AbpAuthorize]
    public class EmployeeAppService : AsyncCrudAppService<
        Employee,
        EmployeeDto,
        long,
        PagedEmployeeResultRequestDto,
        CreateEmployeeDto,
        EmployeeDto>,
        IEmployeeAppService
    {
        private readonly IFlowEngine _flowEngine;

        public EmployeeAppService(IRepository<Employee, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Employee_Read;
            GetAllPermissionName = PermissionNames.Employee_Read;
            CreatePermissionName = PermissionNames.Employee_Create;
            UpdatePermissionName = PermissionNames.Employee_Update;
            DeletePermissionName = PermissionNames.Employee_Delete;
        }

        protected override IQueryable<Employee> CreateFilteredQuery(PagedEmployeeResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.User)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.FullName != null && x.FullName.Contains(input.Keyword)) ||
                    (x.EmployeeCode != null && x.EmployeeCode.Contains(input.Keyword)) ||
                    (x.Department != null && x.Department.Contains(input.Keyword)) ||
                    (x.Title != null && x.Title.Contains(input.Keyword)))
                .WhereIf(!input.FullName.IsNullOrWhiteSpace(), x => x.FullName != null && x.FullName.Contains(input.FullName))
                .WhereIf(!input.EmployeeCode.IsNullOrWhiteSpace(), x => x.EmployeeCode != null && x.EmployeeCode.Contains(input.EmployeeCode))
                .WhereIf(!input.Department.IsNullOrWhiteSpace(), x => x.Department != null && x.Department.Contains(input.Department))
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), x => x.Title != null && x.Title.Contains(input.Title))
                .WhereIf(input.HireDate.HasValue, x => x.HireDate == input.HireDate.Value)
                .WhereIf(input.AnnualLeaveBalance.HasValue, x => x.AnnualLeaveBalance == input.AnnualLeaveBalance.Value)
                .WhereIf(input.HireDateFrom.HasValue, x => x.HireDate >= input.HireDateFrom.Value)
                .WhereIf(input.HireDateTo.HasValue, x => x.HireDate <= input.HireDateTo.Value)
                .WhereIf(input.AnnualLeaveBalanceFrom.HasValue, x => x.AnnualLeaveBalance >= input.AnnualLeaveBalanceFrom.Value)
                .WhereIf(input.AnnualLeaveBalanceTo.HasValue, x => x.AnnualLeaveBalance <= input.AnnualLeaveBalanceTo.Value)
                .WhereIf(input.UserId.HasValue, x => x.UserId == input.UserId.Value);
        }

        public override async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Employee", result);
            return result;
        }

        public override async Task<EmployeeDto> UpdateAsync(EmployeeDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Employee", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Employee", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.Employee_Read)]
        public List<GroupCountDto> GetGroupedCount(EmployeeGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "UserId" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "UserId":
                    return query
                        .GroupBy(x => new { Key = x.UserId, Label = x.User == null ? null : x.User.Name })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                default:
                    return new List<GroupCountDto>();
            }
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.Employee_Read)]
        public decimal? GetStats(EmployeeStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new[] { "HireDate" };
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "AnnualLeaveBalance" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "AnnualLeaveBalance": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.AnnualLeaveBalance)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.AnnualLeaveBalance)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.AnnualLeaveBalance)
                            : query.Average(x => (decimal?)x.AnnualLeaveBalance);
                        default: return null;
            }
        }

        /// <summary>
        /// Rapor verisi — kok kayit ve alt koleksiyonlar TEK yanitta.
        /// PDF sablonu template basina tek apiBinding kullaniyor.
        /// </summary>
        [Abp.Authorization.AbpAuthorize(PermissionNames.Employee_Read)]
        public async Task<EmployeeReportDto> GetReportData(long id)
        {
            var root = await Repository.GetAll()
                .Include(x => x.LeaveRequests)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (root == null)
                throw new Abp.UI.UserFriendlyException($"Kayit bulunamadi: {id}");

            return new EmployeeReportDto
            {
                Data = ObjectMapper.Map<EmployeeDto>(root),
                LeaveRequests = ObjectMapper.Map<List<LeaveRequestDto>>(
                    root.LeaveRequests == null ? new List<LeaveRequest>() : root.LeaveRequests.ToList()),
            };
        }

    }
}
