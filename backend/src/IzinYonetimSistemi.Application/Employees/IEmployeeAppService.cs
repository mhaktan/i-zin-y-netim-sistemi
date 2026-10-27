using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IzinYonetimSistemi.Analytics.Dto;
using IzinYonetimSistemi.Employees.Dto;

namespace IzinYonetimSistemi.Employees
{
    public interface IEmployeeAppService : IAsyncCrudAppService<
        EmployeeDto,
        long,
        PagedEmployeeResultRequestDto,
        CreateEmployeeDto,
        EmployeeDto>
    {
        List<GroupCountDto> GetGroupedCount(EmployeeGroupedCountInput input);
        decimal? GetStats(EmployeeStatsInput input);
        Task<EmployeeReportDto> GetReportData(long id);
    }
}
