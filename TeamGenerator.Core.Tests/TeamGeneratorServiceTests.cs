using AssignTeams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AssignTeams.Core.UnitTests
{
    public class TeamGeneratorServiceTests
    {
        private readonly ITeamGeneratorService _generator;
        private List<Team> _createdTeams;

        public TeamGeneratorServiceTests()
        {
            _generator = new TeamGeneratorService();
        }

        [Fact]
        public void GeneratesSameNumberOfTeamsAsPeopleWhenMaxPeoplePerTeamIsOne()
        {
            // Arrange
            int maxPeoplePerTeam = 1;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        MaximumPeoplePerTeam = maxPeoplePerTeam,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.MaximumPeoplePerTeam
                    };
            int expectedNumberOfTeams = userOptions.People.Count;

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }

        [Fact]
        public void GeneratesEvenTeamSizesWhenNumberOfPeoplesIsDivisibleByMaxPeoplePerTeam()
        {
            // Arrange
            int maxPeoplePerTeam = 3;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        MaximumPeoplePerTeam = maxPeoplePerTeam,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.MaximumPeoplePerTeam
                    };
            int expectedNumberOfTeams = userOptions.People.Count / maxPeoplePerTeam;

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[0].People.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[1].People.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[2].People.Count);
        }

        [Theory]
        [InlineData(2,5)]
        [InlineData(5,2)]
        [InlineData(4,3)]
        public void GeneratesTeamsRoundedToNextWholeNumberWhenNumberOfPeopleIsNotDivisibleByMaxPeoplePerTeam(int maxPeoplePerTeam, int expectedNumberOfTeams)
        {
            // Arrange
            GeneratorParams userOptions = new GeneratorParams
            {
                MaximumPeoplePerTeam = maxPeoplePerTeam,
                People = TestData.NinePeople,
                RandomizeBy = RandomizeBy.MaximumPeoplePerTeam
            };

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }

        [Fact]
        public void GeneratesOneTeamWhenNumberOfTeamsIsOne()
        {
            // Arrange
            int expectedNumberOfTeams = 1;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        TotalNumberOfTeams = expectedNumberOfTeams,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }

        [Fact]
        public void GeneratesUnevenTeamSizesWhenNumberOfPeopleIsNotDivisibleByNumberOfTeamsRequested()
        {
            // Arrange
            int numberOfTeams = 2;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        TotalNumberOfTeams = numberOfTeams,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedTotalNumberOfPeople = userOptions.People.Count;

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();
            int teamOneCount = _createdTeams[0].People.Count;
            int teamTwoCount = _createdTeams[1].People.Count;

            // Assert
            Assert.NotEqual(teamOneCount, teamTwoCount);
            Assert.Equal(expectedTotalNumberOfPeople, teamOneCount + teamTwoCount);
        }

        [Fact]
        public void GeneratesEvenTeamsWhenNumberOfPeopleIsDivisibleByNumberOfTeamsRequested()
        {
            // Arrange
            int expectedNumberOfTeams = 3;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        TotalNumberOfTeams = expectedNumberOfTeams,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedPeoplePerTeam = userOptions.People.Count/expectedNumberOfTeams;

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[0].People.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[1].People.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[2].People.Count);
        }

        [Fact]
        public void GeneratesOnePersonPerTeamWhenNumberOfPeopleEqualsNumberOfTeamsRequested()
        {
            // Arrange
            int expectedNumberOfTeams = 9;
            GeneratorParams userOptions = new GeneratorParams
                    {
                        TotalNumberOfTeams = expectedNumberOfTeams,
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedPeoplePerTeam = userOptions.People.Count / expectedNumberOfTeams;

            // Act
            _createdTeams = _generator.Run(userOptions).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[0].People.Count);
        }

        [Fact]
        public void GeneratesExceptionWhenGeneratorParametersIsNull()
        {
            // Arrange
            GeneratorParams userOptions = null;

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
            Assert.True(ex.Message.Contains("The parameters required to generate random teams is null."));
        }

        [Fact]
        public void GeneratesExceptionWhenListOfPeopleIsNull()
        {
            // Arrange
            GeneratorParams userOptions = new GeneratorParams { People = null };

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
            Assert.True(ex.Message.Contains("The list of people to generate into teams is null."));
        }

        [Fact]
        public void GeneratesExceptionWhenListOfPeopleCountIsZero()
        {
            // Arrange
            GeneratorParams userOptions = new GeneratorParams
                    {
                        People = new List<Person>(),
                    };

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "There are no people to generate into teams.");
        }

        [Fact]
        public void GeneratesExceptionWhenMaximumPeoplePerTeamIsZeroForRequestByPeoplePerTeam()
        {
            // Arrange
            GeneratorParams userOptions = new GeneratorParams
                    {
                        People = TestData.NinePeople,
                        RandomizeBy = RandomizeBy.MaximumPeoplePerTeam
                    };

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "The number of people per team cannot be 0 for the generator type selected.");
        }

        [Fact]
        public void GeneratesExceptionWhenNumberOfTeamsIsZeroForRequestByNumberOfTeams()
        {
            // Arrange
            GeneratorParams userOptions = new GeneratorParams
            {
                People = TestData.NinePeople,
                RandomizeBy = RandomizeBy.TotalNumberOfTeams
            };

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "The number of teams cannot be 0 for the generator type selected.");
        }

        [Fact]
        public void GeneratesExceptionWhenNumberOfTeamsRequestedIsGreaterThanNumberOfPeople()
        {
            // Arrange
            int numberOfTeams = 10;
            GeneratorParams userOptions = new GeneratorParams
            {
                TotalNumberOfTeams = numberOfTeams,
                People = TestData.NinePeople,
                RandomizeBy = RandomizeBy.TotalNumberOfTeams
            };

            // Act
            var ex = Record.Exception(() => _generator.Run(userOptions));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "There are not enough people provided for the number of teams requested.");
        }
    }
}
