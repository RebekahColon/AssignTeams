using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AssignTeams.Core.Models;
using AssignTeams.Core;

namespace AssignTeams.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITeamGeneratorService _generatorService;

        public IndexModel()
        {
            _generatorService = new TeamGeneratorService();
        }

        [BindProperty]
        public GeneratorParams GeneratorParameters { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                GeneratorParameters.People.Add(new Person { Name = "Becks" });
                GeneratorParameters.People.Add(new Person { Name = "B" });
                GeneratorParameters.People.Add(new Person { Name = "R" });
                GeneratorParameters.People.Add(new Person { Name = "Colon" });
                _generatorService.Run(GeneratorParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToPage("./ShowTeams");
        }
    }
}
