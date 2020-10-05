using Bigai.Holidays.Core.Domain.Interfaces.Services.Countries;
using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Core.Domain.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ServicesTests.CountriesServicesTests
{
    public class ImportCountryServiceTests
    {
        [Theory]
        [InlineData("Countries.csv")]
        public void Import_MustImportCountriesFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportCountryService countryService = Helper.GetImportCountryService();

            //
            // Act
            //
            var result = countryService.Import(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<Country>> countries = (List<List<Country>>)result.Data;
                Assert.NotNull(countries);
                Assert.True(countries.Count > 0);
                for (int i = 0, j = countries.Count; i < j; i++)
                {
                    Assert.True(countries[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("CountriesErrorColumns.csv")]
        [InlineData("CountriesErrorFormat.pdf")]
        [InlineData("CountriesNotFound.csv")]
        public void Import_MustImportCountriesFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportCountryService countryService = Helper.GetImportCountryService();

            //
            // Act
            //
            var result = countryService.Import(filename);

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("Countries.csv")]
        public async Task ImportAsync_MustImportCountriesFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportCountryService countryService = Helper.GetImportCountryService();

            //
            // Act
            //
            var result = await countryService.ImportAsync(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<Country>> countries = (List<List<Country>>)result.Data;
                Assert.NotNull(countries);
                Assert.True(countries.Count > 0);
                for (int i = 0, j = countries.Count; i < j; i++)
                {
                    Assert.True(countries[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("CountriesErrorColumns.csv")]
        [InlineData("CountriesErrorFormat.pdf")]
        [InlineData("CountriesNotFound.csv")]
        public async Task ImportAsync_MustImportCountriesFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportCountryService countryService = Helper.GetImportCountryService();

            //
            // Act
            //
            var result = await countryService.ImportAsync(filename);

            // Assert
            Assert.False(result.Success);
        }
    }
}
