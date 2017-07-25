using System;
using System.Collections.Generic;
using System.Text;
using AssignTeams.Core.Models;
using System.Linq;

namespace AssignTeams.Core
{
    public class TeamGeneratorService : ITeamGeneratorService
    {
        private readonly Random _randomizer;
        public TeamGeneratorService()
        {
            _randomizer = new Random();
        }

        public IEnumerable<Team> Run(GeneratorParams parameters)
        {
            // Create team objects and set the team numbers based on players per team
            List<Team> teams = new List<Team>();
            for (int i = 0; i < parameters.NumberOfTeams; i++)
            {
                teams.Add(new Team() { Number = i + 1 });
            }

            // Use max number of teams and associates per team to create the number of team number selections
            List<int> availableTeamNumbers = new List<int>();
            for (int i = 0; i < parameters.AssociatesPerTeam; i++)
            {
                for (int j = 0; j < parameters.NumberOfTeams; j++)
                {
                    availableTeamNumbers.Add(j + 1);
                }
            }

            // Randomly assign an associate to a team
            int numberToLoop = parameters.NumberOfTeams * parameters.AssociatesPerTeam;
            for (int i = 0; i < numberToLoop; i++)
            {
                // Generate a team number at random out of the available teams remaining, then remove that item from future draws
                int randomIndex = _randomizer.Next(0, availableTeamNumbers.Count);
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
