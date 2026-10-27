using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IzinYonetimSistemi.Analytics.Dto;
using IzinYonetimSistemi.LeaveTypes.Dto;

namespace IzinYonetimSistemi.LeaveTypes
{
    public interface ILeaveTypeAppService : IAsyncCrudAppService<
        LeaveTypeDto,
        long,
        PagedLeaveTypeResultRequestDto,
        CreateLeaveTypeDto,
        LeaveTypeDto>
    {
        decimal? GetStats(LeaveTypeStatsInput input);
        Task<LeaveTypeReportDto> GetReportData(long id);
    }
}
