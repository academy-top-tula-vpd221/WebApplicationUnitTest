using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        IStore store;
        public HomeController(IStore store)
        {
            this.store = store;
        }

        public IActionResult Index()
        {
            return View(store.GetAll());
        }

        public IActionResult GetEmployee(int? id)
        {
            if (!id.HasValue) return BadRequest();
            Employee employee = store.GetById(id.Value);

            if(employee is not null)
                return View(employee);
            else
                return NotFound();
        }

        public IActionResult AddEmployee() => View();

        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if(ModelState.IsValid)
            {
                store.Create(employee);
                return RedirectToAction("Index");
            }
            return View(employee);
        }
    }
}
