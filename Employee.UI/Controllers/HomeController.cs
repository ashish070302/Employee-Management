using Employee.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using EmployeeRepositories.Interface;
using Employee.UI.ViewModels.HomePageViewModel;

namespace Employee.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepo _employeeRepo;

        public HomeController(ILogger<HomeController> logger, IEmployeeRepo employeeRepo)
        {
            _logger = logger;
            _employeeRepo = employeeRepo;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogIn", "Account");
            }

            var emps = await _employeeRepo.GetAll();
            var vm = emps.Select(x => new HomeEmployeeViewModel
            {
                EmployeeId = x.Id,
                EmployeeName = x.Name,
                EmployeeImage = x.ImageURL,
                EmployeeDept = x.Department.Name
            }).ToList();

            return View(vm);

            //var emps = await _employeeRepo.GetAll();
            //var vm = emps.Select(x => new HomeEmployeeViewModel
            //{
            //    EmployeeId = x.Id, 
            //    EmployeeName = x.Name,
            //    EmployeeImage = x.ImageURL,
            //    EmployeeDept = x.Department.Name
            //}).ToList();
            //return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id)
        {
            var emp = await _employeeRepo.GetById(id);
            if(emp == null)
            {
                return NotFound();
            }
            var vm = new HomeEmployeeDetailsViewModel
            {
                EmpId = emp.Id,
                EmpCode = emp.Code,
                EmpName = emp.Name,
                EmpAadhar = emp.Aadhar,
                EmpAddress = emp.Address,
                EmpPhoneNumber = emp.PhoneNumber,
                EmpEmail = emp.Email,
                DateOfBirth = emp.DateOfBirth,
                DateOfJoining = emp.DateOfJoining,
                ImageURL = emp.ImageURL,
                DeptartmentName = emp.Department.Name,
                StateName = emp.State.Name,
                CityName = emp.City.Name
            };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
