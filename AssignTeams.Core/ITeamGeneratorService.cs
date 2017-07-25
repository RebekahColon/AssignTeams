using AssignTeams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssignTeams.Core
{
    public interface ITeamGeneratorService
    {
        IEnumerable<Team> Run(GeneratorParams parameters);
    }
}
