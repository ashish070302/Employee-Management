using Employee.UI.ViewModels;
using EmployeeEntity;
using EmployeeRepositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class DepartmentController : Controller
    {
        public IDepartmentRepo _departmentRepo;

        public DepartmentController(IDepartmentRepo departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }
        public async Task<IActionResult> Index()
        {
            List<DepartmentViewModel> vm = new List<DepartmentViewModel>();

            var departments = await _departmentRepo.GetAll();

            foreach (var dept in departments)
            {
                vm.Add(new DepartmentViewModel
                {
                    Id = dept.Id,
                    Name = dept.Name
                });
            }

            return View(vm);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentViewModel vm)
        {
            var dept = new Department
            {
                Name = vm.Name
            };
            await _departmentRepo.Save(dept);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var dept = await _departmentRepo.GetById(id);
            EditDepartmentViewModel vm = new EditDepartmentViewModel
            {
                Id = dept.Id,
                Name = dept.Name
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditDepartmentViewModel vm)
        {
            var dept = await _departmentRepo.GetById(vm.Id);
            dept.Name = vm.Name;

            await _departmentRepo.Edit(dept);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _departmentRepo.GetById(id);
            await _departmentRepo.RemoveData(dept);
            return RedirectToAction("Index");
        }
    }
}
