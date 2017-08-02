using System;
using System.Collections.Generic;
using System.Text;
using AssignTeams.Core.Models;
using System.Linq;

namespace AssignTeams.Core
{
    public class TeamGeneratorService : ITeamGeneratorService
    {
        private readonly Random _randomNumber;
        public TeamGeneratorService()
        {
            _randomNumber = new Random();
        }

        public IEnumerable<Team> Run(GeneratorParams parameters)
        {
            // TODO: Add Fail fast checks
            // 1. Divide by zero based on the randomizer
            // 2. The below check if selection is number of teams
            //if (parameters.Associates.Count < parameters.NumberOfTeams)
            //    throw new ArgumentException("There are not enough team members provided for the number of teams requested.");

            int totalPeopleProvided = parameters.Associates.Count;
            int totalNumberOfTeams;

            // Determine number of teams based on randomizer
            switch (parameters.RandomizeBy)
            {
                case RandomizeBy.PeoplePerTeam:
                    totalNumberOfTeams = totalPeopleProvided / parameters.AssociatesPerTeam;
                    break;
                case RandomizeBy.TotalNumberOfTeams:
                default:
                    totalNumberOfTeams = parameters.NumberOfTeams;
                    break;
            }

            // Create team objects and set the team numbers based on players per team
            List<Team> teams = new List<Team>();
            for (int i = 0; i < totalNumberOfTeams; i++)
            {
                teams.Add(new Team() { Number = i + 1 });
            }

            // Create the assignable list of teams based on the number of teams and number of people provided to split into the teams
            List<int> availableTeamNumbers = new List<int>();
            int teamNumber = 1;
            for (int i = 0; i < totalPeopleProvided; i++)
            {
                availableTeamNumbers.Add(teamNumber);
                teamNumber++;
                if (teamNumber == totalNumberOfTeams)
                    teamNumber = 1; //reset the team number
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
