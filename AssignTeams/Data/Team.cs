using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssignTeams.Data
{
    public class Team
    {
        public int Number { get; set; }
        public List<Associate> Members { get; set; }

        public Team()
        {
            Members = new List<Associate>();
        }
    }
}
