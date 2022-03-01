using Food_Application.Data;
using Food_Application.Models;
using Food_Application.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Food_Application.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _dbContext;
        public OrderController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //GET Checkout actioin method

        public IActionResult Checkout()
        {
            return View();
        }

        //POST Checkout action method

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Items> items = HttpContext.Session.Get<List<Items>>("items");
            if (items != null)
            {
                foreach (var product in items)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.PorductId = product.Id;
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }

            anOrder.OrderNo = GetOrderNo();
            _dbContext.Orders.Add(anOrder);
            await _dbContext.SaveChangesAsync();
            HttpContext.Session.Set("products", new List<Items>());
            return View();
        }


        public string GetOrderNo()
        {
            int rowCount = _dbContext.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        
    }
}
