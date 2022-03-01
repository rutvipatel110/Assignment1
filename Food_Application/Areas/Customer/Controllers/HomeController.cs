using Food_Application.Data;
using Food_Application.Models;
using Food_Application.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using X.PagedList;

namespace Food_Application.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        
        private  ApplicationDbContext _dbContext;
        public HomeController(ApplicationDbContext dbContext )
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int? page)
        {
            return View(_dbContext.Items.Include(c => c.FoodItems).Include(c => c.SpecialTag).ToList().ToPagedList(page ?? 1, 9));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //GET product detail acation method

        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _dbContext.Items.Include(c => c.FoodItems).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST product detail acation method
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id)
        {
            List<Items> items = new List<Items>();
            if (id == null)
            {
                return NotFound();
            }

            var item = _dbContext.Items.Include(c => c.FoodItems).FirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            items = HttpContext.Session.Get<List<Items>>("items");
            if (items == null)
            {
                items = new List<Items>();
            }
            items.Add(item);
            HttpContext.Session.Set("items", items);
            return RedirectToAction(nameof(Index));
        }
        //GET Remove action method
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Items> items = HttpContext.Session.Get<List<Items>>("items");
            if (items != null)
            {
                var item = items.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
                    items.Remove(item);
                    HttpContext.Session.Set("items", items);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]

        public IActionResult Remove(int? id)
        {
            List<Items> items = HttpContext.Session.Get<List<Items>>("items");
            if (items != null)
            {
                var item = items.FirstOrDefault(c => c.Id == id);
                if (item != null)
                {
                    items.Remove(item);
                    HttpContext.Session.Set("items", items);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        //GET product Cart action method

        public IActionResult Cart()
        {
            List<Items> items = HttpContext.Session.Get<List<Items>>("items");
            if (items == null)
            {
                items = new List<Items>();
            }
            return View(items);
        }



    }
}
