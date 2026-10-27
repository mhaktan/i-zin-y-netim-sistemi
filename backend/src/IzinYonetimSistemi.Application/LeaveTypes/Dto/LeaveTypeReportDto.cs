using System;
using System.Collections.Generic;
using IzinYonetimSistemi.LeaveRequests.Dto;

namespace IzinYonetimSistemi.LeaveTypes.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class LeaveTypeReportDto
    {
        public LeaveTypeDto Data { get; set; }
        public List<LeaveRequestDto> LeaveRequests { get; set; }
    }
}
