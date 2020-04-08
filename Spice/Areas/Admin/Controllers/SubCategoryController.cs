using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        [TempData]
        public string StatusMessage { get; set; }

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
                    StatusMessage = "Error : This subcategory already exists under " + checkIfSubCategoryExists.First().category.Name + " category. Please use a different name for the subcategory.";
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
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(), 
                StatusMessage = StatusMessage
            };

            return View(newModel);
        }

        //GET - EDIT

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var subcategory = await _db.SubCategory.SingleOrDefaultAsync(k => k.Id == id);
            if (subcategory == null)
            {

                return NotFound();
            }


            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subcategory,
                SubCategoryList = await _db.SubCategory.OrderBy(k => k.Name).Select(l => l.Name).ToListAsync()
            };
            return View(model);

        }

        //POST - EDIT

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id, SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var checkIfSubCategoryExists = _db.SubCategory.Include(j => j.category).Where(j => j.Name == model.SubCategory.Name && j.category.Id == model.SubCategory.CategoryId);

                if (checkIfSubCategoryExists.Count() > 0)
                {
                    //error
                    StatusMessage = "Error : This subcategory already exists under " + checkIfSubCategoryExists.First().category.Name + " category. Please use a different name for the subcategory.";
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
                SubCategoryList = await _db.SubCategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };

            return View(newModel);
        }


        //GET - LIST OF SUBCATEGORIES

        [ActionName("GetSubcategory")]
        public async Task<IActionResult> GetSubcategory(int id) 
        {
            List<SubCategory> Subcategories = new List<SubCategory>();
            Subcategories = await (from SubCategory in _db.SubCategory
                                   where SubCategory.CategoryId == id
                                   select SubCategory).ToListAsync();

            return Json(new SelectList(Subcategories, "Id", "Name"));
        }
    }
}
