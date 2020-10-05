using Bigai.Holidays.Core.Domain.Interfaces.Services.States;
using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Core.Domain.Tests.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ServicesTests.StatesServicesTests
{
    public class ImportStatesServiceTests
    {

        [Theory]
        [InlineData("StatesUnitedStates.csv")]
        public void Import_MustImportStatesFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportStateService stateService = Helper.GetImportStateService();

            //
            // Act
            //
            var result = stateService.Import(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<State>> states = (List<List<State>>)result.Data;
                Assert.NotNull(states);
                Assert.True(states.Count > 0);
                for (int i = 0, j = states.Count; i < j; i++)
                {
                    Assert.True(states[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("StatesErrorColumnsUnitedStates.csv")]
        [InlineData("StatesErrorFormatUnitedStates.pdf")]
        [InlineData("StatesUnitedStates.csv")]
        public void Import_MustImportStatesFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportStateService stateService = Helper.GetImportStateService();

            //
            // Act
            //
            var result = stateService.Import(filename);

            // Assert
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData("StatesUnitedStates.csv")]
        public async Task ImportAsync_MustImportStatesFromCsvFile_True(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportStateService stateService = Helper.GetImportStateService();

            //
            // Act
            //
            var result = await stateService.ImportAsync(filename);

            // Assert
            Assert.True(result.Success);

            if (result.Success)
            {
                List<List<State>> states = (List<List<State>>)result.Data;
                Assert.NotNull(states);
                Assert.True(states.Count > 0);
                for (int i = 0, j = states.Count; i < j; i++)
                {
                    Assert.True(states[i].Count > 0);
                }
            }
        }

        [Theory]
        [InlineData("StatesErrorColumnsUnitedStates.csv")]
        [InlineData("StatesErrorFormatUnitedStates.pdf")]
        [InlineData("StatesUnitedStates.csv")]
        public async Task ImportAsync_MustImportStatesFromCsvFile_False(string file)
        {
            // Arrange
            string filename = Helper.GetFilePath(file);
            IImportStateService stateService = Helper.GetImportStateService();

            //
            // Act
            //
            var result = await stateService.ImportAsync(filename);

            // Assert
            Assert.False(result.Success);
        }
    }
}
