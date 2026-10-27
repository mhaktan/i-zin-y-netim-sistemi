using System;
using IzinYonetimSistemi.Analytics.Dto;

namespace IzinYonetimSistemi.LeaveTypes.Dto
{
    /// <summary>GetAll ile ayni filtreleri kabul eder, ustune GroupBy alir.</summary>
    public class LeaveTypeGroupedCountInput : PagedLeaveTypeResultRequestDto
    {
        public string GroupBy { get; set; }
    }

    public class LeaveTypeStatsInput : PagedLeaveTypeResultRequestDto
    {
        /// <summary>avg | sum | min | max | avgDayDiff</summary>
        public string Aggregate { get; set; }
        public string Field { get; set; }
        public string FromField { get; set; }
        public string ToField { get; set; }
    }
}
