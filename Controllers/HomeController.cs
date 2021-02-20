using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using JustManAdmin.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace JustManAdmin.Controllers
{
    public class HomeController : BaseHomeController
    {
        private readonly ILogger<HomeController> _logger;
        private DataContext context;
        public HomeController(ILogger<HomeController> logger, DataContext _context)
        {
            this.context = _context;
            _logger = logger;
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MainCategory model)
        {
            context.MainCategories.Add(model);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.MainCategories.FirstAsync(p => p.Id == id);
            var articles = await context.Articles
                .Include(p => p.MainCategory)
                .Where(p => p.MainCategory.Id == id)
                .ToListAsync();
            context.Articles.RemoveRange(articles);
            context.MainCategories.Remove(model);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.MainCategories.FirstAsync(p => p.Id == id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditM(MainCategory model, IFormFile img)
        {
            var oldModel = await context.MainCategories.FirstAsync(p => p.Id == model.Id);
            oldModel.Name = model.Name;
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
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
