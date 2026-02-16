using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;
using System.Threading.Tasks;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private IRepositoryEmpleados repo;
        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<VistaEmpleadoDepartamento> empleados = await this.repo.GetEmpleadosDepartamentoAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int idEmp)
        {
            VistaEmpleadoDepartamento empleado = await this.repo.GetDetallesEmpleadosDepartamentoAsync(idEmp);
            return View(empleado);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Empleado model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await this.repo.InsertEmpleadoAsync(model.Apellido, model.Oficio, model.Dir, model.Salario, model.Comision, model.NombreDept);
            return RedirectToAction("Index");
        }
    }
}