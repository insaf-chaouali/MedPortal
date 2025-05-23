using Microsoft.EntityFrameworkCore;
using projet_1.Data;
using projet_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projet_1.Services
{
    public class DossierMedicalService
    {
        private readonly ApplicationDbContext _context;

        public DossierMedicalService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all appointments
        public async Task<IEnumerable<DossierMedical>> GetAllDossierMedicalAsync()
        {
            return await _context.DossierMedical
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .ToListAsync();
        }
        // Get an appointment by ID
        public async Task<DossierMedical> GetDossierMedicalByIdAsync(int id)
        {
            return await _context.DossierMedical
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        // Create an appointment
        public async Task<DossierMedical> CreateDossierMedicalAsync(DossierMedical dossierMedical)
        {
            if (dossierMedical.PatientId == dossierMedical.MedecinId)
            {
                throw new InvalidOperationException("A user cannot book an medical folder with themselves.");
            }

            _context.DossierMedical.Add(dossierMedical);
            await _context.SaveChangesAsync();
            return dossierMedical;
        }
        // Update an appointment
        public async Task<bool> UpdateDossierMedicalAsync(DossierMedical dossierMedical)
        {
            var existingDossierMedical = await _context.DossierMedical
                                                    .AsNoTracking()  // Avoid tracking this entity
                                                    .FirstOrDefaultAsync(r => r.Id == dossierMedical.Id);
            if (existingDossierMedical == null)
                return false;

            // Detach any tracked instance before attaching the updated entity
            _context.Entry(existingDossierMedical).State = EntityState.Detached;
            _context.Entry(dossierMedical).State = EntityState.Modified;

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
        public async Task<bool> DeleteDossierMedicalAsync(int id)
        {
            var dossierMedical = await _context.DossierMedical.FindAsync(id);
            if (dossierMedical == null)
                return false;

            _context.DossierMedical.Remove(dossierMedical);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
