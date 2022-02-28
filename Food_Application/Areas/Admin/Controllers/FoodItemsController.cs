using Food_Application.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Food_Application.Models;

namespace Food_Application.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FoodItemsController : Controller
    {
        private ApplicationDbContext _dbContext;
        public FoodItemsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            //var data = _dbContext.FoodItems.ToList()
            return View(_dbContext.FoodItems.ToList());
        }
        //Create Get Action Method
        public ActionResult Create()
        {
            return View();
        }
        //Create Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodItems foodItems)
        {
            if (ModelState.IsValid)
            {
                _dbContext.FoodItems.Add(foodItems);
                await _dbContext.SaveChangesAsync();
                TempData["save"] = "Product type has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(foodItems);
        }

        //Edit Get Action Method
        public ActionResult Edit(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var foodItems = _dbContext.FoodItems.Find(id);
            if (foodItems == null)
            {
                return NotFound();
            }
            return View(foodItems);
        }
        //Edit Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FoodItems foodItems)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Update(foodItems);
                await _dbContext.SaveChangesAsync();
                TempData["edit"] = "Product type has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(foodItems);
        }

        //Details Get Action Method
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foodItems = _dbContext.FoodItems.Find(id);
            if (foodItems == null)
            {
                return NotFound();
            }
            return View(foodItems);
        }
        //Detail Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(FoodItems foodItems)
        {
            return RedirectToAction(nameof(Index));
        }

        //Delete Get Action Method
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var foodItems = _dbContext.FoodItems.Find(id);
            if (foodItems == null)
            {
                return NotFound();
            }
            return View(foodItems);
        }
        //Delete Post Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,FoodItems foodItems)
        {
            if(id== null)
            {
                return NotFound();
            }
            if (id != foodItems.Id)
            {
                return NotFound();
            }
            var foodItem= _dbContext.FoodItems.Find(id);
            if(foodItem == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                _dbContext.Remove(foodItem);
                await _dbContext.SaveChangesAsync();

                TempData["delete"] = "Product type has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(foodItems);
        }
    }
}
