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
                    if(parameters.AssociatesPerTeam == 0) throw new ArgumentException("The number of people per team cannot be 0.");
                    totalNumberOfTeams = totalPeopleProvided / parameters.AssociatesPerTeam;
                    break;
                case RandomizeBy.TotalNumberOfTeams:
                    if (parameters.AssociatesPerTeam == 0) throw new ArgumentException("The number of people per team cannot be 0.");
                    if (parameters.Associates.Count < parameters.NumberOfTeams) throw new ArgumentException("There are not enough team members provided for the number of teams requested.");
                    totalNumberOfTeams = parameters.NumberOfTeams;
                    break;
                default:
                    throw new ArgumentException("The given approach to generate teams is not supported.");
            }

            // Create team objects and set the team numbers based on players per team
            List<Team> teams = new List<Team>();
            for (int i = 0; i < totalNumberOfTeams; i++)
            {
                teams.Add(new Team() { Number = i + 1 });
            }

            // Create the assignable list of teams based on the number of teams and people provided to split into the teams
            List<int> availableTeamNumbers = new List<int>();
            int teamNumber = 1;
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                availableTeamNumbers.Add(teamNumber);
                teamNumber++;
                if (teamNumber == totalNumberOfTeams) teamNumber = 1; //reset in order to loop through the team numbers again from the beginning
            }

            // Randomly assign an associate to a team
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                // Generate a team number at random out of the available teams remaining, then remove that item from future draws
                int randomIndex = _randomNumber.Next(0, availableTeamNumbers.Count);
                Team team = teams.FirstOrDefault(t => t.Number == availableTeamNumbers[randomIndex]);
                availableTeamNumbers.RemoveAt(randomIndex);

                // Assign the current associate to that team, then remove that associate
                team.Members.Add(parameters.Associates[0]);
                parameters.Associates.RemoveAt(0);

                // Break out if there are no more team members to assign
                if (parameters.Associates.Count == 0)
                    break;
            }

            return teams;
        }
    }
}
