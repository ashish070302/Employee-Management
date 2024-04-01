using Employee.UI.ViewModels;
using EmployeeEntity;
using EmployeeRepositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeUI.Controllers
{
    public class StateController : Controller
    {
        public IStateRepo _stateRepo;

        public StateController(IStateRepo stateRepo)
        {
            _stateRepo = stateRepo;
        }
        public async Task<IActionResult> Index()
        {
            List<StateViewModel> vm = new List<StateViewModel>();

            var states = await _stateRepo.GetAll();

            foreach (var state in states)
            {
                vm.Add(new StateViewModel
                {
                    Id = state.Id,
                    Name = state.Name,
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
        public async Task<IActionResult> Create(CreateStateViewModel vm)
        {
            var state = new State
            {
                Name = vm.Name,
            };
            await _stateRepo.Save(state);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var artist = await _stateRepo.GetById(id);
            EditStateViewModel vm = new EditStateViewModel
            {
                Id = artist.Id,
                Name = artist.Name
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditStateViewModel vm)
        {
            var state = await _stateRepo.GetById(vm.Id);
            state.Name = vm.Name;

            await _stateRepo.Edit(state);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var state = await _stateRepo.GetById(id);
            await _stateRepo.RemoveData(state);
            return RedirectToAction("Index");
        }
    }
}
