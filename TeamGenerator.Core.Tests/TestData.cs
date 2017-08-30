using AssignTeams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssignTeams.Core.UnitTests
{
    public class TestData
    {
        public static List<Associate> NineAssociates
        {
            get
            {
                return new List<Associate>()
                {
                    new Associate {Name = "Sarah"},
                    new Associate {Name = "Abraham"},
                    new Associate {Name = "Isaac"},
                    new Associate {Name = "Rebekah"},
                    new Associate {Name = "Rachel"},
                    new Associate {Name = "Jacob"},
                    new Associate {Name = "Noah"},
                    new Associate {Name = "David"},
                    new Associate {Name = "Ruth"}
                };
            }
        }
    }
}
