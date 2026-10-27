using System;
using System.Collections.Generic;
using IzinYonetimSistemi.LeaveRequests.Dto;

namespace IzinYonetimSistemi.Employees.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class EmployeeReportDto
    {
        public EmployeeDto Data { get; set; }
        public List<LeaveRequestDto> LeaveRequests { get; set; }
    }
}
