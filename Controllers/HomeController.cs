using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JustManAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace JustManAdmin.Controllers
{
    public class HomeController : BaseHomeController
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext context;
        public HomeController(ILogger<HomeController> logger,DataContext _context)
        {
            this.context = _context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var li = await context.MainCategories.ToListAsync();
            return View(li);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
