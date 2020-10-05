using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ModelsTests.HolidaysRulesModelsTests
{
    public class HolidayTests
    {
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "2020/10/04", "Local", false, "Native Description", "alternativeDescription", "BRA", "RJ", "Rio de Janeiro", "3304557")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "2020/10/04", "Local", false, "Native Description", "alternativeDescription", "BRA", "RJ", "", "")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "", "2020/10/04", "Local", false, "Native Description", "alternativeDescription", "BRA", "", "", "")]
        public void CreateHoliday_MustCreateInstaceOfHoliday(string id, string status, string typeProcess, string user, string country, string state, string date, string type, bool optional, string nativeDescription, string alternativeDescription, string countryCode, string stateCode, string cityName, string cityCode)
        {
            // Arrange
            Holiday holiday;
            Guid entityId = string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
            EntityStatus entityStatus = EntityStatus.GetByName(status);
            TypeProcess action = TypeProcess.GetByName(typeProcess);
            Guid userId = string.IsNullOrEmpty(user) ? Guid.Empty : Guid.Parse(user);
            Guid countryId = string.IsNullOrEmpty(country) ? Guid.Empty : Guid.Parse(country);
            Guid stateId = string.IsNullOrEmpty(state) ? Guid.Empty : Guid.Parse(state);
            DateTime holidayDate = DateTime.Parse(date);
            HolidayType holidayType = HolidayType.GetByName(type);

            // Act
            holiday = Holiday.CreateHoliday(entityId, entityStatus, action, userId, countryId, stateId, holidayDate, holidayType, optional, nativeDescription, alternativeDescription, countryCode, stateCode, cityName, cityCode);

            // Assert
            Assert.NotNull(holiday);
            Assert.True(holiday.Id != entityId &&
                        holiday.Status == entityStatus &&
                        holiday.Action == action &&
                        (holiday.RegisteredBy == userId || holiday.ModifiedBy == userId) &&
                        holiday.CountryId == countryId &&
                        holiday.StateId == stateId &&
                        holiday.HolidayDate == holidayDate &&
                        holiday.HolidayType == holidayType &&
                        holiday.Optional == optional &&
                        holiday.NativeDescription == nativeDescription &&
                        holiday.AlternativeDescription == alternativeDescription &&
                        holiday.CountryCode == countryCode &&
                        holiday.StateCode == stateCode &&
                        holiday.CityName == cityName &&
                        holiday.CityCode == cityCode);
        }

        [Fact]
        public void EqualsCore_RulesMustBeEquals_True()
        {
            // Arrange
            Holiday holidayA, holidayB;

            // Act
            holidayA = Holiday.CreateHoliday(null, EntityStatus.Active, TypeProcess.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), DateTime.Parse("2020/10/04"), HolidayType.Local, false, "Native Description", "alternativeDescription", "BRA", "RJ", "Rio de Janeiro", "3304557");
            holidayB = Holiday.CreateHoliday(null, EntityStatus.Active, TypeProcess.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), DateTime.Parse("2020/10/04"), HolidayType.Local, false, "Native Description", "alternativeDescription", "BRA", "RJ", "Rio de Janeiro", "3304557");

            // Assert
            Assert.Equal(holidayA, holidayB);
            Assert.True(holidayA == holidayB);
        }

        [Fact]
        public void EqualsCore_RulesMustBeEquals_False()
        {
            // Arrange
            Holiday holidayA, holidayB;

            // Act
            holidayA = Holiday.CreateHoliday(null, EntityStatus.Active, TypeProcess.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7080"), DateTime.Parse("2020/10/04"), HolidayType.Local, false, "Native Description", "alternativeDescription", "BRA", "RJ", "Rio de Janeiro", "3304557");
            holidayB = Holiday.CreateHoliday(null, EntityStatus.Active, TypeProcess.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc8070"), DateTime.Parse("2020/10/04"), HolidayType.Local, false, "Native Description", "alternativeDescription", "BRA", "RJ", "Rio de Janeiro", "3304557");

            // Assert
            Assert.NotEqual(holidayA, holidayB);
            Assert.True(holidayA != holidayB);
        }
    }
}
