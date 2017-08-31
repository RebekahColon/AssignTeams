using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignTeams.Core.Models
{
    public class Team
    {
        public int Number { get; set; }
        public List<Person> People { get; set; }

        public Team()
        {
            People = new List<Person>();
        }
    }
}
