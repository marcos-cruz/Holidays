using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using Xunit;

namespace Bigai.Holidays.Core.Domain.Tests.ModelsTests.StatesModelsTests
{
    public class StateTests
    {
        [Theory]
        [InlineData(null, "Active", "Register", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "BRA", "SP", "São Paulo")]
        [InlineData("00000000-0000-0000-0000-000000000000", "Locked", "Update", "41408960-a65f-42ea-93c0-e320a1bc7060", "41408960-a65f-42ea-93c0-e320a1bc7070", "USA", "AL", "Alabama")]
        public void CreateState_MustCreateInstaceOfState(string id, string status, string typeProcess, string user, string country, string countryIsoCode, string stateIsoCode, string name)
        {
            // Arrange
            State state;
            Guid entityId = string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
            EntityStatus entityStatus = EntityStatus.GetByName(status);
            ActionType action = ActionType.GetByName(typeProcess);
            Guid userId = string.IsNullOrEmpty(user) ? Guid.Empty : Guid.Parse(user);
            Guid countryId = string.IsNullOrEmpty(country) ? Guid.Empty : Guid.Parse(country);

            // Act
            state = State.CreateState(entityId, entityStatus, action, userId, countryId, countryIsoCode, stateIsoCode, name, null);

            // Assert
            Assert.NotNull(state);
            Assert.True(state.Id != entityId &&
                        state.Status == entityStatus &&
                        state.Action == action &&
                        (state.RegisteredBy == userId || state.ModifiedBy == userId) &&
                        state.CountryId == countryId &&
                        state.CountryIsoCode == countryIsoCode &&
                        state.StateIsoCode == stateIsoCode &&
                        state.Name == name);
        }

        [Fact]
        public void EqualsCore_StatesMustBeEquals_True()
        {
            // Arrange
            State stateA, stateB;

            // Act
            stateA = State.CreateState(null, EntityStatus.Active, ActionType.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "BRA", "SP", "São Paulo", null);
            stateB = State.CreateState(null, EntityStatus.Active, ActionType.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "BRA", "SP", "São Paulo", null);

            // Assert
            Assert.Equal(stateA, stateB);
            Assert.True(stateA == stateB);
        }

        [Fact]
        public void EqualsCore_StatesMustBeEquals_False()
        {
            // Arrange
            State stateA, stateB;

            // Act
            stateA = State.CreateState(null, EntityStatus.Active, ActionType.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "BRA", "SP", "São Paulo", null);
            stateB = State.CreateState(null, EntityStatus.Active, ActionType.Register, Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7060"), Guid.Parse("41408960-a65f-42ea-93c0-e320a1bc7070"), "USA", "AL", "Alabama", null);

            // Assert
            Assert.NotEqual(stateA, stateB);
            Assert.True(stateA != stateB);
        }
    }
}
