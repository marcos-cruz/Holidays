using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ModelsTests.CountriesModelsTests
{
    public class CountryTests
    {
        [Theory]
        [InlineData(null, "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "4", "AF", "AFG", "Afghanistan", "Afghanistan", "fa, ps", "Asia", "Southern Asia", "", 142, 34, 0)]
        [InlineData("00000000-0000-0000-0000-000000000000", "Locked", "Update", "41408960-a65f-42ea-93c0-e320a1bc7060", "248", "AX", "ALA", "Åland Islands", "Åland Islands", "sv", "Europe", "Northern Europe", "", 150, 154, 0)]
        public void CreateCountry_MustCreateInstaceOfCountry(string id, string status, string typeProcess, string user, string numericCode, string alphaIsoCode2, string alphaIsoCode3, string name, string shortName, string languageCode, string regionName, string subRegionName, string intermediateRegionName, int regionCode, int subRegionCode, int intermediateRegionCode)
        {
            // Arrange
            Country country;
            Guid entityId = string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
            EntityStatus entityStatus = EntityStatus.GetByName(status);
            ActionType action = ActionType.GetByName(typeProcess);
            Guid userId = string.IsNullOrEmpty(user) ? Guid.Empty : Guid.Parse(user);

            // Act
            country = Country.CreateCountry(entityId, entityStatus, action, userId, numericCode, alphaIsoCode2, alphaIsoCode3, name, shortName, languageCode, regionName, subRegionName, intermediateRegionName, regionCode, subRegionCode, intermediateRegionCode, null);

            // Assert
            Assert.NotNull(country);
            Assert.True(country.Id != entityId &&
                        country.Status == entityStatus &&
                        country.Action == action &&
                        (country.RegisteredBy == userId || country.ModifiedBy == userId) &&
                        country.NumericCode == numericCode &&
                        country.CountryIsoCode2 == alphaIsoCode2 &&
                        country.CountryIsoCode3 == alphaIsoCode3 &&
                        country.Name == name &&
                        country.ShortName == shortName &&
                        country.LanguageCode == languageCode &&
                        country.RegionName == regionName &&
                        country.SubRegionName == subRegionName &&
                        country.IntermediateRegionName == intermediateRegionName &&
                        country.RegionCode == regionCode &&
                        country.IntermediateRegionCode == intermediateRegionCode);
        }

        [Fact]
        public void EqualsCore_CountriesMustBeEquals_True()
        {
            // Arrange
            Country countryA, countryB;

            // Act
            countryA = Country.CreateCountry(null, EntityStatus.Active, ActionType.Register, null, "4", "AF", "AFG", "Afghanistan", "Afghanistan", "fa, ps", "Asia", "Southern Asia", "", 142, 34, 0, null);
            countryB = Country.CreateCountry(null, EntityStatus.Locked, ActionType.Update, null, "4", "AF", "AFG", "Afghanistan", "Afghanistan", "fa, ps", "Asia", "Southern Asia", "", 142, 34, 0, null);

            // Assert
            Assert.Equal(countryA, countryB);
            Assert.True(countryA == countryB);
        }

        [Fact]
        public void EqualsCore_CountriesMustBeEquals_False()
        {
            // Arrange
            Country countryA, countryB;

            // Act
            countryA = Country.CreateCountry(null, EntityStatus.Active, ActionType.Register, null, "4", "AF", "AFG", "Afghanistan", "Afghanistan", "fa, ps", "Asia", "Southern Asia", "", 142, 34, 0, null);
            countryB = Country.CreateCountry(null, EntityStatus.Locked, ActionType.Update, null, "248", "AX", "ALA", "Åland Islands", "Åland Islands", "sv", "Europe", "Northern Europe", "", 150, 154, 0, null);

            // Assert
            Assert.NotEqual(countryA, countryB);
            Assert.True(countryA != countryB);
        }
    }
}
