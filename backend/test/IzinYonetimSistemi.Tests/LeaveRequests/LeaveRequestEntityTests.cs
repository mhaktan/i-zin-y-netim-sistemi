using System;
using Xunit;
using FluentAssertions;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.Tests.LeaveRequests
{
    public class LeaveRequestEntityTests
    {
        [Fact]
        public void LeaveRequest_ShouldBeCreatable()
        {
            // Act
            var entity = new LeaveRequest();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void LeaveRequest_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new LeaveRequest();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void LeaveRequest_RequestNo_ShouldAcceptValue()
        {
            var entity = new LeaveRequest { RequestNo = "Test Value" };
            entity.RequestNo.Should().Be("Test Value");
        }

    }
}
