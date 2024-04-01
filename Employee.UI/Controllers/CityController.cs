using Employee.UI.ViewModels;
using EmployeeEntity;
using EmployeeRepositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace EmployeeUI.Controllers
{
    public class CityController : Controller
    {
        private ICityRepo _cityRepo;
        private IStateRepo _stateRepo;

        public CityController(ICityRepo cityRepo, IStateRepo stateRepo)
        {
            _cityRepo = cityRepo;
            _stateRepo = stateRepo;
        }

        public async Task<IActionResult> Index()
        {
            List<CityViewModel> vm = new List<CityViewModel>();

            var cities = await _cityRepo.GetAll();

            foreach (var city in cities)
            {
                vm.Add(new CityViewModel
                {
                    Id = city.Id,
                    Name = city.Name,
                    StateName =  city.State.Name
                });
            }
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var states = await _stateRepo.GetAll();
            ViewBag.stateList = new SelectList(states, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCityViewModel vm)
        {
            var city = new City
            {
                Name = vm.Name,
                StateId = vm.StateId,
            };
            await _cityRepo.Save(city);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var states = await _stateRepo.GetAll();
            ViewBag.stateList = new SelectList(states, "Id", "Name");

            var city = await _cityRepo.GetById(id);
            var vm = new EditCityViewModel
            {
                Id = city.Id,
                Name = city.Name,
                StateId = city.StateId,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCityViewModel vm)
        {
            var city = await _cityRepo.GetById(vm.Id);
            city.Id = vm.Id;
            city.Name = vm.Name;
            city.StateId = vm.StateId;
            
            await _cityRepo.Edit(city);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var city = await _cityRepo.GetById(id);
            await _cityRepo.RemoveData(city);
            return RedirectToAction("Index");
        }
    }
}
