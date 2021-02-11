using System.Linq;
using System;
using JustManAdmin.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustManAdmin.Controllers
{
    [Produces("application/json")]
    [Route("api/mobile")]
    [ApiController]
    public class MobileApisController : ControllerBase
    {
        private DataContext context;
        public MobileApisController(DataContext _context)
        {
            this.context = _context;
        }
        [HttpGet("getMain")]
        public async Task<JsonResult> GetMainCategories()
        {
            var li = await context.MainCategories.ToListAsync();
            return new JsonResult(li);
        }
        [HttpGet("getArticles/{id}")]
        public async Task<JsonResult> GetArticles(int id)
        {
            var li = await context.Articles
            .Include(p=>p.MainCategory)
            .Where(p=>
                p.MainCategory.Id == id)
                .ToListAsync();
            return new JsonResult(li);
        }
        [HttpGet("getArticle/{id}")]
        public async Task<JsonResult> GetArticle(int id)
        {
            var model = await context.Articles
                .FirstOrDefaultAsync(p=>
                p.Id == id);
            return new JsonResult(model);
        }
    }
}