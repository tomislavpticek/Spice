using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models.ViewModels;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public SubCategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET - INDEX

        public async Task<IActionResult> Index()
        {
            var subCategories = await _db.SubCategory.Include(s => s.category).ToListAsync();
            return View(subCategories);
        }

        //GET - CREATE

        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await _db.SubCategory.OrderBy(k => k.Name).Select(l => l.Name).ToListAsync()
            };
            return View(model);

        }

        //POST - CREATE

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkIfSubCategoryExists = _db.SubCategory.Include(j => j.category).Where(j => j.Name == model.SubCategory.Name && j.category.Id == model.SubCategory.CategoryId);

                if (checkIfSubCategoryExists.Count() > 0)
                {
                    //error
                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }



            }
            SubCategoryAndCategoryViewModel newModel = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync()
            };

            return View(newModel);
        }
    }
}
