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
        PeoplePerTeam
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
            if(parameters == null) throw new ArgumentNullException("The parameters provided to generate teams is null.");
            if(parameters.Associates == null) throw new ArgumentNullException("The associates to generate into teams is null.");
            if(parameters.Associates.Count == 0) throw new ArgumentException("There are no associates to generate into teams.");

            int totalPeopleProvided = parameters.Associates.Count;
            int totalNumberOfTeams;
            switch (parameters.RandomizeBy)
            {
                case RandomizeBy.PeoplePerTeam:
                    if(parameters.AssociatesPerTeam == 0) throw new ArgumentException($"The number of people per team cannot be 0 {generateTypeMessage}.");
                    totalNumberOfTeams = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalPeopleProvided) / Convert.ToDecimal(parameters.AssociatesPerTeam)));
                    break;
                case RandomizeBy.TotalNumberOfTeams:
                default:
                    if (parameters.NumberOfTeams == 0) throw new ArgumentException($"The number of teams cannot be 0 {generateTypeMessage}.");
                    if (parameters.Associates.Count < parameters.NumberOfTeams) throw new ArgumentException("There are not enough team members provided for the number of teams requested.");
                    totalNumberOfTeams = parameters.NumberOfTeams;
                    break;
            }

            // Create team objects and set the team numbers based on players per team
            List<Team> teams = new List<Team>();
            for (int i = 0; i < totalNumberOfTeams; i++)
            {
                teams.Add(new Team() { Number = i + 1 });
            }

            // Create the assignable list of teams based on the number of teams and people provided to split into the teams
            List<int> availableTeamNumbers = new List<int>();
            int currentTeamNumber = 1;
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                // Check if reset is needed to loop through the team numbers again from the beginning
                if (currentTeamNumber > totalNumberOfTeams) currentTeamNumber = 1;

                availableTeamNumbers.Add(currentTeamNumber);
                currentTeamNumber++;
            }

            // Randomly assign an associate to a team
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                // Generate a team number at random out of the available teams remaining 
                int randomIndex = _randomNumber.Next(0, availableTeamNumbers.Count);
                Team team = teams.FirstOrDefault(t => t.Number == availableTeamNumbers[randomIndex]);
                if (team == null) throw new Exception("There was an error creating your teams.  Please try again later.");

                // Assign the current associate to that team 
                team.Members.Add(parameters.Associates[0]);

                // Then remove that associate and team number occurrence from future draws
                parameters.Associates.RemoveAt(0);
                availableTeamNumbers.RemoveAt(randomIndex);

                // Break out if there are no more team members to assign
                if (parameters.Associates.Count == 0)
                    break;
            }

            return teams;
        }
    }
}
