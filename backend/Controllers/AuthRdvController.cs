using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projet_1.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Logging; // Add this for logging

namespace projet_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only authenticated users can access this controller
    public class AuthRdvController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthRdvController> _logger; // Add a logger for the controller

        // Inject the logger in the constructor
        public AuthRdvController(ApplicationDbContext context, ILogger<AuthRdvController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("medecin")]
        public IActionResult GetRendezVousByMedecin()
        {
            // Get the user ID from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Console.WriteLine(userIdClaim);
            if (userIdClaim == null)
            {
                // Log the warning if the user is not authenticated
                _logger.LogWarning("User not authenticated.");
                return Unauthorized("User not authenticated.");
            }

            // Log the user ID for informational purposes
            _logger.LogInformation($"User ID: {userIdClaim.Value}");

            // Parse the user ID safely
            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Invalid user ID.");
                return Unauthorized("Invalid user ID.");
            }

            // Retrieve appointments for this doctor only (assuming MedecinId matches userId)
            var rendezVousList = _context.RendezVous
                .Where(r => r.MedecinId == userId)
                .Include(r => r.Patient) // Include related Patient data
                .ToList();

            if (!rendezVousList.Any())
            {
                _logger.LogInformation("No appointments found for the connected doctor.");
                return NotFound("No appointments found for the connected doctor.");
            }

            // Return the list of appointments
            return Ok(rendezVousList);
        }
    }
}
