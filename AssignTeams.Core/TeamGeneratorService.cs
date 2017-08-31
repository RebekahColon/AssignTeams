using System;
using System.Collections.Generic;
using System.Text;
using AssignTeams.Core.Models;
using System.Linq;

namespace AssignTeams.Core
{
    public enum RandomizeBy
    {
        TotalNumberOfTeams,
        MaximumPeoplePerTeam
    }

    public class TeamGeneratorService : ITeamGeneratorService
    {
        private const string generateTypeMessage = "for the generator type selected";
        private readonly Random _randomNumber;
        public TeamGeneratorService()
        {
            _randomNumber = new Random();
        }

        public IEnumerable<Team> Run(GeneratorParams parameters)
        {
            // Guard clauses
            if(parameters == null) throw new ArgumentNullException("The parameters required to generate random teams is null.");
            if(parameters.People == null) throw new ArgumentNullException("The list of people to generate into teams is null.");
            if(parameters.People.Count == 0) throw new ArgumentException("There are no people to generate into teams.");

            int totalPeopleProvided = parameters.People.Count;
            int totalNumberOfTeams;
            switch (parameters.RandomizeBy)
            {
                case RandomizeBy.MaximumPeoplePerTeam:
                    if(parameters.MaximumPeoplePerTeam == 0) throw new ArgumentException($"The number of people per team cannot be 0 {generateTypeMessage}.");
                    totalNumberOfTeams = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalPeopleProvided) / Convert.ToDecimal(parameters.MaximumPeoplePerTeam)));
                    break;
                case RandomizeBy.TotalNumberOfTeams:
                default:
                    if (parameters.TotalNumberOfTeams == 0) throw new ArgumentException($"The number of teams cannot be 0 {generateTypeMessage}.");
                    if (parameters.People.Count < parameters.TotalNumberOfTeams) throw new ArgumentException("There are not enough people provided for the number of teams requested.");
                    totalNumberOfTeams = parameters.TotalNumberOfTeams;
                    break;
            }

            // Create team objects and set the team numbers based on players per team
            List<Team> teams = new List<Team>();
            for (int i = 0; i < totalNumberOfTeams; i++)
            {
                teams.Add(new Team() { Number = i + 1 });
            }

            // Create a list of team numbers available for randomly assigning to each person
            List<int> availableTeamNumbers = new List<int>();
            int currentTeamNumber = 1;
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                // Check if reset is needed to loop through the team numbers again from the beginning
                if (currentTeamNumber > totalNumberOfTeams) currentTeamNumber = 1;

                availableTeamNumbers.Add(currentTeamNumber);
                currentTeamNumber++;
            }

            // Randomly assign a person to a team
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                // Generate a team number at random out of the available teams remaining 
                int randomIndex = _randomNumber.Next(0, availableTeamNumbers.Count);
                Team team = teams.FirstOrDefault(t => t.Number == availableTeamNumbers[randomIndex]);

                // If for some reason team is null, stop processing
                if (team == null) throw new Exception("There was an error creating your teams.  Please try again later.");

                // Assign the current person to randomly selected team 
                team.People.Add(parameters.People[0]);

                // Then remove that person and team number occurrence from future draws
                parameters.People.RemoveAt(0);
                availableTeamNumbers.RemoveAt(randomIndex);

                // Break out if there are no more people to assign to a team
                if (parameters.People.Count == 0)
                    break;
            }

            return teams;
        }
    }
}
