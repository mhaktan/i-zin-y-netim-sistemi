using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using IzinYonetimSistemi.Analytics.Dto;
using IzinYonetimSistemi.StateMachine.Dto;
using IzinYonetimSistemi.LeaveRequests.Dto;

namespace IzinYonetimSistemi.LeaveRequests
{
    public interface ILeaveRequestAppService : IAsyncCrudAppService<
        LeaveRequestDto,
        long,
        PagedLeaveRequestResultRequestDto,
        CreateLeaveRequestDto,
        LeaveRequestDto>
    {
        Task<LeaveRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);

        // Yalniz ApprovalAppService cagirir — REST'e acik degil
        [Abp.Application.Services.RemoteService(false)]
        Task<LeaveRequestDto> ApplyApprovalTransitionAsync(long id, ChangeStatusInput input);
        List<GroupCountDto> GetGroupedCount(LeaveRequestGroupedCountInput input);
        decimal? GetStats(LeaveRequestStatsInput input);
    }
}
