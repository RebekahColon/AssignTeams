using AssignTeams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssignTeams.Core.UnitTests
{
    public class TestData
    {
        public static List<Person> NinePeople
        {
            get
            {
                return new List<Person>()
                {
                    new Person {Name = "Sarah"},
                    new Person {Name = "Abraham"},
                    new Person {Name = "Isaac"},
                    new Person {Name = "Rebekah"},
                    new Person {Name = "Rachel"},
                    new Person {Name = "Jacob"},
                    new Person {Name = "Noah"},
                    new Person {Name = "David"},
                    new Person {Name = "Ruth"}
                };
            }
        }
    }
}
