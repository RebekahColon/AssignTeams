using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AssignTeams.Core.Models;

namespace AssignTeams.Pages
{
    public class ShowTeamsModel : PageModel
    {

        public void OnGet()
        {
            var list = ViewData["teams"] as List<Team>;
        }

        public ActionResult ShowTeams()
        {
            return null;
        }
    }
}