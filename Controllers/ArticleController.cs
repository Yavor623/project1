using AccountManagement.Data;
using AccountManagement.Models;
using AccountManagement.Models.Articles;
using AccountManagement.Models.Rating;
using AccountManagement.Models.Trains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Controllers
{
	public class ArticleController : Controller
	{
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ArticleController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
		{
            var articles = _db.Articles.ToList();
			return View(articles);
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Id = model.Id,
                    TimeItWasAdded = DateTime.Now,
                    Content = model.Content,
                    Title = model.Title
                };
                _db.Articles.Add(article);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var theArticle = _db.Articles.FirstOrDefault(a => a.Id == id);
            var article = new EditArticleViewModel
            {
                Id = theArticle.Id,
                TimeItWasAdded = theArticle.TimeItWasAdded,
                Content = theArticle.Content,
                Title = theArticle.Title
            };

            return View(article);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var article = _db.Articles.FirstOrDefault(a => a.Id == id);

                article.Id = model.Id;
                article.TimeItWasAdded = model.TimeItWasAdded;
                article.Content = model.Content;
                article.Title = model.Title;
                article.TimeItWasAdded = DateTime.Now;
                _db.Articles.Update(article);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var theArticle = _db.Articles.FirstOrDefault(a => a.Id == id);
            var article = new DeleteArticleViewModel
            {
                Id = theArticle.Id,
                Title = theArticle.Title,
                Content = theArticle.Content,
            };

            return View(article);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.Id == id);
            if (article != null)
            {
                _db.Articles.Remove(article);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
