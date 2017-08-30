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
        private GeneratorParams _params;
        private List<Team> _createdTeams;

        public TeamGeneratorServiceTests()
        {
            _generator = new TeamGeneratorService();
        }

        [Fact]
        public void GeneratesSameNumberOfTeamsAsAssociatesWhenMaxPeoplePerTeamIsOne()
        {
            // Arrange
            int maxPeoplePerTeam = 1;
            _params = new GeneratorParams
                    {
                        AssociatesPerTeam = maxPeoplePerTeam,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.PeoplePerTeam
                    };
            int expectedNumberOfTeams = _params.Associates.Count;

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }

        [Fact]
        public void GeneratesEvenTeamSizesWhenNumberOfAssociatesIsDivisibleByMaxPeoplePerTeam()
        {
            // Arrange
            int maxPeoplePerTeam = 3;
            _params = new GeneratorParams
                    {
                        AssociatesPerTeam = maxPeoplePerTeam,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.PeoplePerTeam
                    };
            int expectedNumberOfTeams = _params.Associates.Count / maxPeoplePerTeam;

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[0].Members.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[1].Members.Count);
            Assert.Equal(maxPeoplePerTeam, _createdTeams[2].Members.Count);
        }

        [Theory]
        [InlineData(2,5)]
        [InlineData(5,2)]
        [InlineData(4,3)]
        public void GeneratesNumberOfTeamsRoundedToNextWholeNumberWhenNumberOfAssociatesIsNotDivisibleByMaxPeoplePerTeam(int maxPeoplePerTeam, int expectedNumberOfTeams)
        {
            // Arrange
            _params = new GeneratorParams
            {
                AssociatesPerTeam = maxPeoplePerTeam,
                Associates = TestData.NineAssociates,
                RandomizeBy = RandomizeBy.PeoplePerTeam
            };

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }


        [Fact]
        public void GeneratesOneTeamWhenNumberOfTeamsIsOne()
        {
            // Arrange
            int expectedNumberOfTeams = 1;
            _params = new GeneratorParams
                    {
                        NumberOfTeams = expectedNumberOfTeams,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
        }

        [Fact]
        public void GeneratesUnevenTeamSizesWhenNumberOfPeopleIsNotDivisibleByNumberOfTeamsRequested()
        {
            // Arrange
            int numberOfTeams = 2;
            _params = new GeneratorParams
                    {
                        NumberOfTeams = numberOfTeams,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedTotalNumberOfPeople = _params.Associates.Count;

            // Act
            _createdTeams = _generator.Run(_params).ToList();
            int teamOneCount = _createdTeams[0].Members.Count;
            int teamTwoCount = _createdTeams[1].Members.Count;

            // Assert
            Assert.NotEqual(teamOneCount, teamTwoCount);
            Assert.Equal(expectedTotalNumberOfPeople, teamOneCount + teamTwoCount);
        }

        [Fact]
        public void GeneratesEvenTeamsWhenNumberOfPeopleIsDivisibleByNumberOfTeamsRequested()
        {
            // Arrange
            int expectedNumberOfTeams = 3;
            _params = new GeneratorParams
                    {
                        NumberOfTeams = expectedNumberOfTeams,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedPeoplePerTeam = _params.Associates.Count/expectedNumberOfTeams;

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[0].Members.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[1].Members.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[2].Members.Count);
        }

        [Fact]
        public void GeneratesOnePersonPerTeamWhenNumberOfPeopleEqualsNumberOfTeamsRequested()
        {
            // Arrange
            int expectedNumberOfTeams = 9;
            _params = new GeneratorParams
                    {
                        NumberOfTeams = expectedNumberOfTeams,
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.TotalNumberOfTeams
                    };
            int expectedPeoplePerTeam = _params.Associates.Count / expectedNumberOfTeams;

            // Act
            _createdTeams = _generator.Run(_params).ToList();

            // Assert
            Assert.Equal(expectedNumberOfTeams, _createdTeams.Count);
            Assert.Equal(expectedPeoplePerTeam, _createdTeams[0].Members.Count);
        }

        [Fact]
        public void GeneratesExceptionWhenParametersIsNull()
        {
            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
            Assert.True(ex.Message.Contains("The parameters provided to generate teams is null."));
        }

        [Fact]
        public void GeneratesExceptionWhenAssociatesIsNull()
        {
            // Arrange
            _params = new GeneratorParams
                    {
                        Associates = null,
                    };

            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentNullException>(ex);
            Assert.True(ex.Message.Contains("The associates to generate into teams is null."));
        }

        [Fact]
        public void GeneratesExceptionWhenAssociatesCountIsZero()
        {
            // Arrange
            _params = new GeneratorParams
                    {
                        Associates = new List<Associate>(),
                    };

            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "There are no associates to generate into teams.");
        }

        [Fact]
        public void GeneratesExceptionWhenNumberOfAssociatesPerTeamIsZeroForRequestByAssociatesPerTeam()
        {
            // Arrange
            _params = new GeneratorParams
                    {
                        Associates = TestData.NineAssociates,
                        RandomizeBy = RandomizeBy.PeoplePerTeam
                    };

            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "The number of people per team cannot be 0 for the generator type selected.");
        }

        [Fact]
        public void GeneratesExceptionWhenNumberOfTeamsIsZeroForRequestByNumberOfTeams()
        {
            // Arrange
            _params = new GeneratorParams
            {
                Associates = TestData.NineAssociates,
                RandomizeBy = RandomizeBy.TotalNumberOfTeams
            };

            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "The number of teams cannot be 0 for the generator type selected.");
        }

        [Fact]
        public void GeneratesExceptionWhenNumberOfTeamsRequestedIsGreaterThanTotalAssociates()
        {
            // Arrange
            int numberOfTeams = 10;
            _params = new GeneratorParams
            {
                NumberOfTeams = numberOfTeams,
                Associates = TestData.NineAssociates,
                RandomizeBy = RandomizeBy.TotalNumberOfTeams
            };

            // Act
            var ex = Record.Exception(() => _generator.Run(_params));

            // Assert
            Assert.NotNull(ex);
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal(ex.Message, "There are not enough team members provided for the number of teams requested.");
        }
    }
}
