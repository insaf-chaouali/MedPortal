using Microsoft.EntityFrameworkCore;
using projet_1.Data;
using projet_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projet_1.Services
{
    public class RendezVousService
    {
        private readonly ApplicationDbContext _context;

        public RendezVousService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all appointments
        public async Task<IEnumerable<RendezVous>> GetAllRendezVousAsync()
        {
            return await _context.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .ToListAsync();
        }
        // Get an appointment by ID
        public async Task<RendezVous> GetRendezVousByIdAsync(int id)
        {
            return await _context.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        // Create an appointment
        public async Task<RendezVous> CreateRendezVousAsync(RendezVous rendezVous)
        {
            if (rendezVous.PatientId == rendezVous.MedecinId)
            {
                throw new InvalidOperationException("A user cannot book an appointment with themselves.");
            }

            _context.RendezVous.Add(rendezVous);
            await _context.SaveChangesAsync();
            return rendezVous;
        }
        // Update an appointment
        public async Task<bool> UpdateRendezVousAsync(RendezVous rendezVous)
        {
            var existingRendezVous = await _context.RendezVous
                                                    .AsNoTracking()  // Avoid tracking this entity
                                                    .FirstOrDefaultAsync(r => r.Id == rendezVous.Id);
            if (existingRendezVous == null)
                return false;

            // Detach any tracked instance before attaching the updated entity
            _context.Entry(existingRendezVous).State = EntityState.Detached;
            _context.Entry(rendezVous).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception if the entity has been modified by someone else
                return false;
            }
        }
        // Delete an appointment
        public async Task<bool> DeleteRendezVousAsync(int id)
        {
            var rendezVous = await _context.RendezVous.FindAsync(id);
            if (rendezVous == null)
                return false;

            _context.RendezVous.Remove(rendezVous);
            await _context.SaveChangesAsync();
            return true;
        }
        //filtre par date
        public async Task<List<RendezVous>> GetRendezVousByDateAsync(DateTime date)
        {
            return await _context.RendezVous
                .Where(rv => rv.Date.Date == date.Date)
                .ToListAsync();
        }
        //filtre par etat
        public async Task<List<RendezVous>> GetRendezVousByEtatAsync(string etat)
        {
            return await _context.RendezVous
                .Where(rv => rv.Etat.Equals(etat, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}
