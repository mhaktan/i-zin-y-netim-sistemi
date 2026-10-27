using System;
using Xunit;
using FluentAssertions;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.Tests.LeaveTypes
{
    public class LeaveTypeEntityTests
    {
        [Fact]
        public void LeaveType_ShouldBeCreatable()
        {
            // Act
            var entity = new LeaveType();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void LeaveType_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new LeaveType();

            // Assert
            entity.Id.Should().Be(default(long));
            entity.RequiresDocument.Should().Be(false);
        }

        [Fact]
        public void LeaveType_Name_ShouldAcceptValue()
        {
            var entity = new LeaveType { Name = "Test Value" };
            entity.Name.Should().Be("Test Value");
        }

    }
}
