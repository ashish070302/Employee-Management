using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class StateController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
