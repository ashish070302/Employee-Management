using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
