using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TuProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : ControllerBase
    {
        private readonly PresupuestosRepository _repository;

        public PresupuestoController(PresupuestosRepository repository)
        {
            _repository = repository;
        }



        [HttpGet]
        public ActionResult<List<Presupuestos>> GetAll()
        {
            var presupuestos = _repository.GetAllPresupuestos();

            if (presupuestos == null || presupuestos.Count == 0)
                return NotFound("No hay presupuestos registrados.");

            return Ok(presupuestos);
        }
    }
}
