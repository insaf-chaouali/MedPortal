using projet_1.Data;
using projet_1.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace projet_1.Services
{
    public class AuthRdvService
    {
        private readonly ApplicationDbContext _context;

        public AuthRdvService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get appointments by connected Medecin ID
        public async Task<IEnumerable<RendezVous>> GetRendezVousByMedecinAsync(int medecinId)
        {
            return await _context.RendezVous
                .Where(r => r.MedecinId == medecinId) // Filter by MedecinId
                .Include(r => r.Patient)  // Include Patient details
                .Include(r => r.Medecin)  // Include Medecin details
                .ToListAsync();
        }

        // Other methods remain unchanged
    }
}
