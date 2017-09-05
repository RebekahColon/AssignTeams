using System;
using System.Collections.Generic;
using System.Text;

namespace AssignTeams.Core.Models
{
    public class GeneratorParams
    {
        public IList<Person> People { get; set; }
        public int TotalNumberOfTeams { get; set; } 
        public int? MaximumPeoplePerTeam { get; set; }
        public RandomizeBy RandomizeBy { get; set; }

        public GeneratorParams()
        {
            People = new List<Person>();
        }
    }
}
