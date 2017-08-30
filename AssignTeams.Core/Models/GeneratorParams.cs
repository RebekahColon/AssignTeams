using System;
using System.Collections.Generic;
using System.Text;

namespace AssignTeams.Core.Models
{
    public class GeneratorParams
    {
        public IList<Associate> Associates { get; set; }
        public int NumberOfTeams { get; set; }
        public int AssociatesPerTeam { get; set; }
        public RandomizeBy RandomizeBy { get; set; }

        public GeneratorParams()
        {
            Associates = new List<Associate>();
        }
    }
}
