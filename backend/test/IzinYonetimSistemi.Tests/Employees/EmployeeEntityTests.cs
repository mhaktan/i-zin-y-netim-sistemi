using System;
using Xunit;
using FluentAssertions;
using IzinYonetimSistemi.Entities;

namespace IzinYonetimSistemi.Tests.Employees
{
    public class EmployeeEntityTests
    {
        [Fact]
        public void Employee_ShouldBeCreatable()
        {
            // Act
            var entity = new Employee();

            // Assert
            entity.Should().NotBeNull();
        }

        [Fact]
        public void Employee_ShouldHaveDefaultValues()
        {
            // Act
            var entity = new Employee();

            // Assert
            entity.Id.Should().Be(default(long));

        }

        [Fact]
        public void Employee_FullName_ShouldAcceptValue()
        {
            var entity = new Employee { FullName = "Test Value" };
            entity.FullName.Should().Be("Test Value");
        }

        [Fact]
        public void Employee_EmployeeCode_ShouldAcceptValue()
        {
            var entity = new Employee { EmployeeCode = "Test Value" };
            entity.EmployeeCode.Should().Be("Test Value");
        }

    }
}
