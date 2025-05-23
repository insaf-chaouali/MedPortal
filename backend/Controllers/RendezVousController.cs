using Microsoft.AspNetCore.Mvc;
using projet_1.Models;
using projet_1.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace projet_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly RendezVousService _rendezVousService;

        public RendezVousController(RendezVousService rendezVousService)
        {
            _rendezVousService = rendezVousService;
        }

        // Get all appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RendezVous>>> GetRendezVous()
        {
            return Ok(await _rendezVousService.GetAllRendezVousAsync());
        }

        // Get an appointment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<RendezVous>> GetRendezVous(int id)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByIdAsync(id);
            if (rendezVous == null) return NotFound();
            return Ok(rendezVous);
        }

        // Create an appointment
        [HttpPost]
        public async Task<ActionResult<RendezVous>> CreateRendezVous(RendezVous rendezVous)
        {
            try
            {
                var createdRendezVous = await _rendezVousService.CreateRendezVousAsync(rendezVous);
                return CreatedAtAction(nameof(GetRendezVous), new { id = createdRendezVous.Id }, createdRendezVous);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an appointment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRendezVous(int id, RendezVous rendezVous)
        {
            if (id != rendezVous.Id) return BadRequest("The appointment ID does not match.");

            var result = await _rendezVousService.UpdateRendezVousAsync(rendezVous);

            if (!result) return NotFound("The appointment was not found or could not be updated.");

            // Return a success message along with a 200 OK status
            return Ok(new { message = "Appointment updated successfully." });
        }


        // Delete an appointment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRendezVous(int id)
        {
            var result = await _rendezVousService.DeleteRendezVousAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // Filtrer par date (format attendu: yyyy-MM-dd)
        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<RendezVous>>> GetRendezVousByDate(DateTime date)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByDateAsync(date);
            if (rendezVous == null || rendezVous.Count == 0) return NotFound("Aucun rendez-vous trouvé pour cette date.");
            return Ok(rendezVous);
        }

        // Filtrer par état
        [HttpGet("etat/{etat}")]
        public async Task<ActionResult<IEnumerable<RendezVous>>> GetRendezVousByEtat(string etat)
        {
            var rendezVous = await _rendezVousService.GetRendezVousByEtatAsync(etat);
            if (rendezVous == null || rendezVous.Count == 0) return NotFound("Aucun rendez-vous trouvé avec cet état.");
            return Ok(rendezVous);
        }
    }
}
