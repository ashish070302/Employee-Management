using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
