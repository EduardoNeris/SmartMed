using System.Threading.Tasks;
using MedicationWebApi.DataAccess;
using MedicationWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using MedicationWebApi.DTO;

namespace MedicationWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly IRepository _repository;

        public MedicationController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Medication>> Create([FromBody] CreateForm createForm)
        {
            if (createForm == null)
                return BadRequest();

            Medication medication = new Medication
            {
                Name = createForm.Name,
                CreationDate = createForm.CreationDate,
                Quantity = createForm.Quantity
            };

            var createdMedication = await _repository.AddMedication(medication);

            return CreatedAtAction(nameof(GetById), new { id = createdMedication.Id }, createdMedication);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Medication>> Delete(int id)
        {
            var medication = await _repository.GetMedicationById(id);

            if (medication == null) return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<Medication>> GetAll()
        {
            return Ok(await _repository.GetMedication());
        }

        [HttpGet("query")]
        public async Task<ActionResult<Medication>> GetById([FromQuery] int id)
        {
            return Ok(await _repository.GetMedicationById(id));
        }
    }
}
