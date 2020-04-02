﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public SubCategoryController (ApplicationDbContext db)
        {
            _db = db;
        }

        //GET - INDEX

        public async Task<IActionResult> Index()
        {
            var subCategories = await _db.SubCategory.Include(s=>s.category).ToListAsync();
            return View(subCategories);
        }
    }
}