using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JustManAdmin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustManAdmin.Controllers
{
    public class ArticleController : BaseHomeController
    {
        private DataContext context;

        public ArticleController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("articles/{id}")]
        public async Task<IActionResult> Articles(int id)
        {
            ViewBag.Id = id;
            var list = await context.Articles
                .Include(p => p.MainCategory)
                .Where(p => p.MainCategory.Id == id)
                .ToListAsync();
            return View(list);
        }

        [HttpGet("addArticle/{MId}")]
        public IActionResult Add(int MId)
        {
            ViewBag.MId = MId;
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddArticle(Article model, IFormFile img)
        {
            var filePath = Path.Combine(Path.GetFullPath("wwwroot/imgs/"), img.FileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await img.CopyToAsync(stream);
            }
            model.ImgPath = "/imgs/" + img.FileName;
            model.MainCategory = await context.MainCategories.FirstAsync(p =>
                p.Id == model.MainCategory.Id);
            context.Articles.Add(model);
            await context.SaveChangesAsync();
            return RedirectToAction("Articles", new { id = model.MainCategory.Id });
        }
        [HttpGet("deleteArticle/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Articles
                .Include(p => p.MainCategory)
                .FirstAsync(p => p.Id == id);
            context.Articles.Remove(model);
            await context.SaveChangesAsync();
            return RedirectToAction("Articles", new { id = model.MainCategory.Id });
        }
        [HttpGet("editArticle/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Articles.FirstAsync(p => p.Id == id);
            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditArticle(Article model, IFormFile img)
        {
            var oldModel = await context.Articles
                .Include(p => p.MainCategory)
                .FirstAsync(p => p.Id == model.Id);
            if (img != null)
            {
                var filePath = Path.Combine(Path.GetFullPath("wwwroot/imgs/"), img.FileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await img.CopyToAsync(stream);
                }
                model.ImgPath = "/imgs/" + img.FileName;
                oldModel.ImgPath = model.ImgPath;
            }
            oldModel.Title = model.Title;
            oldModel.Text = model.Text;
            await context.SaveChangesAsync();
            return RedirectToAction("Articles", new { id = oldModel.MainCategory.Id });
        }
    }
}