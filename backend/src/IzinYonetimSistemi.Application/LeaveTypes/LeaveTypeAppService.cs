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
using IzinYonetimSistemi.LeaveTypes.Dto;
using IzinYonetimSistemi.Analytics.Dto;
using IzinYonetimSistemi.LeaveRequests.Dto;
using IzinYonetimSistemi.Approvals.Dto;
using IzinYonetimSistemi.Authorization;
using IzinYonetimSistemi.Flows;

namespace IzinYonetimSistemi.LeaveTypes
{
    [Abp.Authorization.AbpAuthorize]
    public class LeaveTypeAppService : AsyncCrudAppService<
        LeaveType,
        LeaveTypeDto,
        long,
        PagedLeaveTypeResultRequestDto,
        CreateLeaveTypeDto,
        LeaveTypeDto>,
        ILeaveTypeAppService
    {
        private readonly IFlowEngine _flowEngine;

        public LeaveTypeAppService(IRepository<LeaveType, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.LeaveType_Read;
            GetAllPermissionName = PermissionNames.LeaveType_Read;
            CreatePermissionName = PermissionNames.LeaveType_Create;
            UpdatePermissionName = PermissionNames.LeaveType_Update;
            DeletePermissionName = PermissionNames.LeaveType_Delete;
        }

        protected override IQueryable<LeaveType> CreateFilteredQuery(PagedLeaveTypeResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Name != null && x.Name.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name != null && x.Name.Contains(input.Name))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.MaxDaysPerYear.HasValue, x => x.MaxDaysPerYear == input.MaxDaysPerYear.Value)
                .WhereIf(input.RequiresDocument.HasValue, x => x.RequiresDocument == input.RequiresDocument.Value)
                .WhereIf(input.MaxDaysPerYearFrom.HasValue, x => x.MaxDaysPerYear >= input.MaxDaysPerYearFrom.Value)
                .WhereIf(input.MaxDaysPerYearTo.HasValue, x => x.MaxDaysPerYear <= input.MaxDaysPerYearTo.Value);
        }

        public override async Task<LeaveTypeDto> CreateAsync(CreateLeaveTypeDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "LeaveType", result);
            return result;
        }

        public override async Task<LeaveTypeDto> UpdateAsync(LeaveTypeDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "LeaveType", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "LeaveType", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.LeaveType_Read)]
        public decimal? GetStats(LeaveTypeStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new string[0];
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "MaxDaysPerYear" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "MaxDaysPerYear": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.MaxDaysPerYear)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.MaxDaysPerYear)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.MaxDaysPerYear)
                            : query.Average(x => (decimal?)x.MaxDaysPerYear);
                        default: return null;
            }
        }

        /// <summary>
        /// Rapor verisi — kok kayit ve alt koleksiyonlar TEK yanitta.
        /// PDF sablonu template basina tek apiBinding kullaniyor.
        /// </summary>
        [Abp.Authorization.AbpAuthorize(PermissionNames.LeaveType_Read)]
        public async Task<LeaveTypeReportDto> GetReportData(long id)
        {
            var root = await Repository.GetAll()
                .Include(x => x.LeaveRequests)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (root == null)
                throw new Abp.UI.UserFriendlyException($"Kayit bulunamadi: {id}");

            return new LeaveTypeReportDto
            {
                Data = ObjectMapper.Map<LeaveTypeDto>(root),
                LeaveRequests = ObjectMapper.Map<List<LeaveRequestDto>>(
                    root.LeaveRequests == null ? new List<LeaveRequest>() : root.LeaveRequests.ToList()),
            };
        }

    }
}
