using Bigai.Holidays.Core.Domain.Enums;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ModelsTests.HolidaysRulesModelsTests
{
    public class RuleHolidayTests
    {
        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "BRA", "RJ", "3304557", "Rio de Janeiro", "Local", "Aniversário de Rio de Janeiro", "Rio de Janeiro Creation", 3, 1, false, "", "")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "BRA", "", "", "", "National", "Véspera de Ano Novo (após às 14 horas)", "New Year's Eve (after 14pm)", 12, 31, true, "", "")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Locked", "Update", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "USA", null, "", "", "National", "New Year's Day", "New Year's Day", 1, 1, false, "", "")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Locked", "Update", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "USA", null, "", "", "National", "Bennington Battle Day", "Bennington Battle Day", 8, 16, false, "", "16 August or nearest weekday")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Locked", "Update", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "USA", null, "", "", "National", "New Year's Day", "New Year's Day", 0, 0, false, "18,October,nearest", "18 October or nearest weekday")]
        public void CreateRule_MustCreateInstaceOfRule(string id, string status, string typeProcess, string user, string country, string state, string countryIsoCode, string stateIsoCode, string cityCode, string cityName, string holidayType, string nativeHolidayName, string englishHolidayName, int month, int day, bool optional, string bussinessRule, string comments)
        {
            // Arrange
            RuleHoliday ruleHoliday;
            Guid entityId = string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
            EntityStatus entityStatus = EntityStatus.GetByName(status);
            TypeProcess action = TypeProcess.GetByName(typeProcess);
            HolidayType type = HolidayType.GetByName(holidayType);
            Guid userId = string.IsNullOrEmpty(user) ? Guid.Empty : Guid.Parse(user);
            Guid countryId = string.IsNullOrEmpty(country) ? Guid.Empty : Guid.Parse(country);
            Guid stateId = string.IsNullOrEmpty(state) ? Guid.Empty : Guid.Parse(state);

            // Act
            ruleHoliday = RuleHoliday.CreateRuleHoliday(entityId, entityStatus, action, userId, countryId, stateId, countryIsoCode, stateIsoCode, cityCode, cityName, type, nativeHolidayName, englishHolidayName, month, day, optional, bussinessRule, comments);

            // Assert
            Assert.NotNull(ruleHoliday);
            Assert.True(ruleHoliday.Id != entityId &&
                        ruleHoliday.Status == entityStatus &&
                        ruleHoliday.Action == action &&
                        (ruleHoliday.RegisteredBy == userId || ruleHoliday.ModifiedBy == userId) &&
                        ruleHoliday.CountryId == countryId &&
                        ruleHoliday.StateId == stateId &&
                        ruleHoliday.CountryIsoCode == countryIsoCode &&
                        ruleHoliday.StateIsoCode == stateIsoCode &&
                        ruleHoliday.CityCode == cityCode &&
                        ruleHoliday.CityName == cityName &&
                        ruleHoliday.HolidayType == type &&
                        ruleHoliday.NativeHolidayName == nativeHolidayName &&
                        ruleHoliday.EnglishHolidayName == englishHolidayName &&
                        (ruleHoliday.Month == month || ruleHoliday.Month == null) &&
                        (ruleHoliday.Day == day || ruleHoliday.Day == null) &&
                        ruleHoliday.Optional == optional &&
                        ruleHoliday.BussinessRule == bussinessRule.Replace(" ", "").Trim().ToLower() &&
                        ruleHoliday.Comments == comments);
        }

        [Fact]
        public void EqualsCore_RulesMustBeEquals_True()
        {
            // Arrange
            RuleHoliday ruleA, ruleB;

            // Act
            ruleA = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Locked, TypeProcess.Update, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "USA", null, "", "", HolidayType.National, "Bennington Battle Day", "Bennington Battle Day", 8, 16, false, "", "16 August or nearest weekday");
            ruleB = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Locked, TypeProcess.Update, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "USA", null, "", "", HolidayType.National, "Bennington Battle Day", "Bennington Battle Day", 8, 16, false, "", "16 August or nearest weekday");

            // Assert
            Assert.Equal(ruleA, ruleB);
            Assert.True(ruleA == ruleB);
        }

        [Fact]
        public void EqualsCore_RulesMustBeEquals_False()
        {
            // Arrange
            RuleHoliday ruleA, ruleB;

            // Act
            ruleA = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Locked, TypeProcess.Update, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "USA", null, "", "", HolidayType.National, "Bennington Battle Day", "Bennington Battle Day", 8, 16, false, "", "16 August or nearest weekday");
            ruleB = RuleHoliday.CreateRuleHoliday(null, EntityStatus.Locked, TypeProcess.Update, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "USA", null, "", "", HolidayType.Local, "Bennington Battle Day", "Bennington Battle Day", 8, 16, false, "", "16 August or nearest weekday");

            // Assert
            Assert.NotEqual(ruleA, ruleB);
            Assert.True(ruleA != ruleB);
        }
    }
}
