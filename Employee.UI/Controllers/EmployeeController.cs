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
                
        public async Task<IActionResult> Index(string[] selectedDepartments, string filterText, int pageNumber=1, int pageSize=4, string searchText=null, string sortOrder=null)
        {
            var emps = await _employeeRepo.GetAll();
            var vm = new List<EmployeeViewModel>();

            var departments = await _departmentRepo.GetAll();
            ViewData["departments"] = departments.Select(d => new SelectListItem { Text = d.Name, Value = d.Name });


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


            if (selectedDepartments?.Any() == true && !string.IsNullOrEmpty(searchText))
            {
                emps = emps.Where(e => selectedDepartments.Contains(e.Department.Name) &&
                    (e.Name.Contains(searchText) || e.Code.Contains(searchText)));
            }
            else if (selectedDepartments?.Any() == true)
            {
                emps = emps.Where(e => selectedDepartments.Contains(e.Department.Name));
            }
            else if (!string.IsNullOrEmpty(searchText))
            {
                emps = emps.Where(e => e.Name.Contains(searchText) || e.Code.Contains(searchText));
            }

            //if (!string.IsNullOrEmpty(searchText))
            //{
            //    var searchTerms = searchText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();

            //    // Separate lists for union and intersection filtering
            //    var unionTerms = searchTerms.ToList();
            //    var locationTerms = searchTerms.Where(term => !term.Contains(" ")).ToList(); // Assuming location terms won't have spaces

            //    // Filter by union of search terms (OR condition)
            //    var unionFiltered = emps.Where(x => unionTerms.Any(term =>
            //        x.Name.Contains(term) ||
            //        x.Code.Contains(term) ||
            //        x.PhoneNumber.Contains(term) ||
            //        x.Email.Contains(term) ||
            //        x.Aadhar.Contains(term) ||
            //        x.Department.Name.Contains(term) ||
            //        x.State.Name.Contains(term) ||
            //        x.City.Name.Contains(term)));

            //    // Filter by location terms (independent matches)
            //    var locationFiltered = emps.Where(x => locationTerms.Any(loc =>
            //        x.Department.Name.Contains(loc) ||
            //        x.State.Name.Contains(loc) ||
            //        x.City.Name.Contains(loc)));

            //    // Combine results (union with distinct to avoid duplicates)
            //    emps = unionFiltered.Union(locationFiltered).Distinct();
            //}

            //Sorting
            ////ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "Name" : "";
            //ViewData["CodeSort"] = string.IsNullOrEmpty(sortOrder) ? "Code" : "";
            //ViewData["DepartmentSort"] = sortOrder == "DepartmentName" ? "DepartmentName" : "DepartmentName";
            ////ViewData["CodeSort"] = sortOrder == "Code" ? "Code" : "Code";
            //ViewData["NameSort"] = sortOrder == "Name" ? "Name" : "Name";
            //switch (sortOrder)
            //{
            //    case "Name":
            //        emps = emps.OrderByDescending(x => x.Name).ToList();
            //        break;
            //    case "Code":
            //        emps = emps.OrderByDescending(x => x.Code).ToList();
            //        break;
            //    case "DepartmentName":
            //        emps = emps.OrderByDescending(x => x.Department.Name).ToList();
            //        break;
            //    default:
            //        emps = emps.OrderBy(x => x.Code).ToList();
            //        break;
            //}

            ViewData["SortOrder"] = string.IsNullOrEmpty(sortOrder) ? "Name" : sortOrder;

            // Adjust sorting logic
            switch (sortOrder)
            {
                case "Code":
                    emps = emps.OrderBy(x => x.Code).ToList();
                    break;
                case "DepartmentName":
                    emps = emps.OrderBy(x => x.Department.Name).ToList();
                    break;
                default:
                    // Default sorting by name (ascending)
                    emps = emps.OrderBy(x => x.Name).ToList();
                    break;
            }

            totalItems = emps.ToList().Count();
            emps = emps.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            if (emps.Any())
            {
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
            }
            else
            {
                ViewData["NoResults"] = "No results found.";
            }

            //foreach (var emp in emps)
            //{
            //    vm.Add(new EmployeeViewModel
            //    {
            //        Id = emp.Id,
            //        Name = emp.Name,
            //        Code = emp.Code,
            //        Address = emp.Address,
            //        DateOfBirth = emp.DateOfBirth,
            //        DateOfJoining = emp.DateOfJoining,
            //        Aadhar = emp.Aadhar,
            //        PhoneNumber = emp.PhoneNumber,
            //        Email = emp.Email,
            //        ImageURL = emp.ImageURL,
            //        DeptartmentName = emp.Department.Name,
            //        StateName = emp.State.Name,
            //        CityName = emp.City.Name
            //    });
            //}            

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

            var model = new CreateEmployeeViewModel();
            return View(model);
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