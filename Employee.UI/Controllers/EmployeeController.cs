using Employee.UI.Utility;
using Employee.UI.ViewModels;
using Employee.UI.ViewModels.HomePageViewModel;
using EmployeeEntity;
using EmployeeRepositories;
using EmployeeRepositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EmployeeUI.Controllers
{
    public class EmployeeController : Controller
    {
        public IEmployeeRepo _employeeRepo;
        public IDepartmentRepo _departmentRepo;
        public ICityRepo _cityRepo;
        public IStateRepo _stateRepo;
        public IUtilityRepo _utilityRepo;
        private readonly ApplicationDbContext _context;

        private string containerName = "EmployeeImage";

        public EmployeeController(ApplicationDbContext context, IEmployeeRepo employeeRepo, IDepartmentRepo departmentRepo, ICityRepo cityRepo, IStateRepo stateRepo, IUtilityRepo utilityRepo)
        {
            _context = context;
            _employeeRepo = employeeRepo;
            _departmentRepo = departmentRepo;
            _cityRepo = cityRepo;
            _stateRepo = stateRepo;
            _utilityRepo = utilityRepo;
        }
                
        public async Task<IActionResult> Index(string filterText, int pageNumber=1, int pageSize=2, string searchText=null)
        {
            var emps = await _employeeRepo.GetAll();
            var vm = new List<EmployeeViewModel>();

            if (searchText != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchText = filterText;
            }
            ViewData["filterData"] = searchText;

            int totalItems = 0;
            if (!string.IsNullOrEmpty(searchText))
            {
                emps = emps.Where(x => x.Name.Contains(searchText) ||
                    x.Code.Contains(searchText) ||
                    x.PhoneNumber.Contains(searchText) ||
                    x.Email.Contains(searchText));
            }
            totalItems = emps.ToList().Count;
            emps = emps.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var emp in emps)
            {
                vm.Add(new EmployeeViewModel
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Code = emp.Code,
                    Address = emp.Address,
                    DateOfBirth = emp.DateOfBirth,
                    DateOfJoining = emp.DateOfJoining,
                    Aadhar = emp.Aadhar,
                    PhoneNumber = emp.PhoneNumber,
                    Email = emp.Email,
                    ImageURL = emp.ImageURL,
                    DeptartmentName = emp.Department.Name,
                    StateName = emp.State.Name,
                    CityName = emp.City.Name
                });
            }            

            var pvm = new PageEmployeeViewModel
            {
                Employee = vm,
                PageInfo = new PageInfo
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems,
                }
            };            

            return View(pvm);
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var states = await _context.States.ToListAsync();
            ViewBag.States = new SelectList(states, "Id", "Name");
            ViewBag.Cities = new SelectList(new List<City>(), "Id", "Name");

            var departments = await _departmentRepo.GetAll();
            ViewBag.DepartmentList = new SelectList(departments, "Id", "Name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel vm)
        {

            var emp = new EmployeeEntity.Employee
            {
                Name = vm.Name,
                Code = vm.Code,
                Address = vm.Address,
                DateOfBirth = vm.DateOfBirth,
                DateOfJoining = vm.DateOfJoining,
                Aadhar = vm.Aadhar,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                DepartmentId = vm.DepartmentId,
                StateId = vm.StateId,
                CityId = vm.CityId,
            };

            if (!ModelState.IsValid)
            {
                var states = await _context.States.ToListAsync();
                ViewBag.States = new SelectList(states, "Id", "Name");
                ViewBag.Cities = new SelectList(await _context.Cities.ToListAsync(), "Id", "Name");
                var departments = await _departmentRepo.GetAll();
                ViewBag.DepartmentList = new SelectList(departments, "Id", "Name");

            }

            if (vm.ImageURL != null)
            {
                emp.ImageURL = await _utilityRepo.SaveImage(containerName, vm.ImageURL);
            }
            await _employeeRepo.Save(emp);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {          
            var states = await _stateRepo.GetAll();
            var cities = await _cityRepo.GetAll();
            var departments = await _departmentRepo.GetAll();
            ViewBag.stateList = new SelectList(states, "Id", "Name");
            ViewBag.cityList = new SelectList(cities, "Id", "Name");
            ViewBag.departmentList = new SelectList(departments, "Id", "Name", "JobTitle");

            var emp = await _employeeRepo.GetById(id);

            EditEmployeeViewModel vm = new EditEmployeeViewModel
            {
                Id = emp.Id,
                Name = emp.Name,
                Code = emp.Code,
                Address = emp.Address,
                DateOfBirth = emp.DateOfBirth,
                DateOfJoining = emp.DateOfJoining,
                Aadhar = emp.Aadhar,
                PhoneNumber = emp.PhoneNumber,
                Email = emp.Email,
                ImageURL = emp.ImageURL,
                DepartmentId = emp.DepartmentId,
                StateId = emp.StateId,
                CityId = emp.CityId,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditEmployeeViewModel vm)
        {
            var emp = await _employeeRepo.GetById(vm.Id);

            emp.Id = vm.Id;
            emp.Name = vm.Name;
            emp.Code = vm.Code;
            emp.Address = vm.Address;
            emp.DateOfBirth = vm.DateOfBirth;
            emp.DateOfJoining = vm.DateOfJoining;
            emp.Aadhar = vm.Aadhar;
            emp.PhoneNumber = vm.PhoneNumber;
            emp.Email = vm.Email;
            emp.DepartmentId = vm.DepartmentId;
            emp.StateId = vm.StateId;
            emp.CityId = vm.CityId;

            if (vm.ChooseImage != null)
            {
                emp.ImageURL = await _utilityRepo.EditImage(containerName, vm.ChooseImage, emp.ImageURL);
            }

            await _employeeRepo.Edit(emp);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var emp = await _employeeRepo.GetById(id);
            await _employeeRepo.RemoveData(emp);
            return RedirectToAction("Index");
        }

        public JsonResult GetCityByStateId(int StateId)
        {
            return Json(_context.Cities.Where(u => u.StateId == StateId).ToList());
        }
    }
}
