using Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays;
using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Core.Domain.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ServicesTests.HolidaysRulesServicesTests
{
    public class ImportRuleHolidayServiceTests
    {
        [Theory]
        [InlineData("HolidayBrazil.csv")]
        public void Import_MustImportRulesHolidaysFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportRuleHolidayService ruleHolidayService = Helper.GetImportRuleHolidayService();

            //
            // Act
            //
            var result = ruleHolidayService.Import(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<RuleHoliday>> rulesHolidays = (List<List<RuleHoliday>>)result.Data;
                Assert.NotNull(rulesHolidays);
                Assert.True(rulesHolidays.Count > 0);
                for (int i = 0, j = rulesHolidays.Count; i < j; i++)
                {
                    Assert.True(rulesHolidays[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("HolidayErrorColumnsBrazil.csv")]
        [InlineData("HolidayErrorFormatBrazil.pdf")]
        [InlineData("HolidayNotFound.csv")]
        public void Import_MustImportRulesHolidaysFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportRuleHolidayService ruleHolidayService = Helper.GetImportRuleHolidayService();

            //
            // Act
            //
            var result = ruleHolidayService.Import(filename);

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("HolidayBrazil.csv")]
        public async Task ImportAsync_MustImportRulesHolidaysFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportRuleHolidayService ruleHolidayService = Helper.GetImportRuleHolidayService();

            //
            // Act
            //
            var result = await ruleHolidayService.ImportAsync(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<RuleHoliday>> rulesHolidays = (List<List<RuleHoliday>>)result.Data;
                Assert.NotNull(rulesHolidays);
                Assert.True(rulesHolidays.Count > 0);
                for (int i = 0, j = rulesHolidays.Count; i < j; i++)
                {
                    Assert.True(rulesHolidays[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("HolidayErrorColumnsBrazil.csv")]
        [InlineData("HolidayErrorFormatBrazil.pdf")]
        [InlineData("HolidayNotFound.csv")]
        public async Task ImportAsync_MustImportRulesHolidaysFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportRuleHolidayService ruleHolidayService = Helper.GetImportRuleHolidayService();

            //
            // Act
            //
            var result = await ruleHolidayService.ImportAsync(filename);

            // Assert
            Assert.False(result.Success);
        }
    }
}
