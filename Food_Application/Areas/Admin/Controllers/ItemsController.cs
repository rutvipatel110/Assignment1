using Food_Application.Data;
using Food_Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Food_Application.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemsController : Controller
    {
        private ApplicationDbContext _dbContext;
        
        private IHostingEnvironment _he;

        
        public ItemsController(ApplicationDbContext dbContext,IHostingEnvironment he)
        {
            _dbContext = dbContext;
            _he = he;

        }
        public IActionResult Index()
        {
            //var data = _dbContext.Items.ToList()
            return View(_dbContext.Items.Include(c=>c.FoodItems).Include(f=>f.SpecialTag).ToList());
        }
        //POST Index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var items = _dbContext.Items.Include(c => c.FoodItems).Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                items = _dbContext.Items.Include(c => c.FoodItems).Include(c => c.SpecialTag).ToList();
            }
            return View(items);
        }


        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_dbContext.FoodItems.ToList(), "Id", "ProductTypeId");
            ViewData["TagId"] = new SelectList(_dbContext.SpecialTag.ToList(), "Id", "Name");
            return View();
        }


        //Post Create method
        [HttpPost]
        public async Task<IActionResult> Create(Items items, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _dbContext.Items.FirstOrDefault(c => c.Name == items.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["productTypeId"] = new SelectList(_dbContext.FoodItems.ToList(), "Id", "ProductTypeId");
                    ViewData["TagId"] = new SelectList(_dbContext.SpecialTag.ToList(), "Id", "Name");
                    return View(items);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    items.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    items.Image = "Images/no-image-icon-23485.png";
                }
                _dbContext.Items.Add(items);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(items);
        }
        //GET Edit Action Method

        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_dbContext.FoodItems.ToList(), "Id", "ProductTypeId");
            ViewData["TagId"] = new SelectList(_dbContext.SpecialTag.ToList(), "Id", "Name");
            if (id == null)
            {
                return NotFound();
            }

            var product = _dbContext.Items.Include(c => c.FoodItems).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST Edit Action Method
        [HttpPost]
        public async Task<IActionResult> Edit(Items items, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    items.Image = "Images/" + image.FileName;
                }

                if (image == null)
                {
                    items.Image = "Images/no-image-icon-23485.PNG";
                }
                _dbContext.Items.Update(items);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(items);
        }

        //GET Details Action Method
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var items = _dbContext.Items.Include(c => c.FoodItems).Include(c => c.SpecialTag)
                .FirstOrDefault(c => c.Id == id);
            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }
        //GET Delete Action Method

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = _dbContext.Items.Include(c => c.SpecialTag).Include(c => c.FoodItems).Where(c => c.Id == id).FirstOrDefault();
            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        //POST Delete Action Method

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var items = _dbContext.Items.FirstOrDefault(c => c.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            _dbContext.Items.Remove(items);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}


   
