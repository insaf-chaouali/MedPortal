using Microsoft.EntityFrameworkCore;
using projet_1.Data;
using projet_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projet_1.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all appointments
        public async Task<IEnumerable<Notification>> GetAllNotificationAsync()
        {
            return await _context.Notification
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .ToListAsync();
        }
        // Get an appointment by ID
        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            return await _context.Notification
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        // Create an appointment
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            if (notification.Reciver == notification.Sender)
            {
                throw new InvalidOperationException("A user cannot book an appointment with themselves.");
            }

            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        // Update an appointment
        public async Task<bool> UpdateNotificationAsync(Notification notification)
        {
            var existingNotification = await _context.Notification
                                                    .AsNoTracking()  // Avoid tracking this entity
                                                    .FirstOrDefaultAsync(r => r.Id == notification.Id);
            if (existingNotification == null)
                return false;

            // Detach any tracked instance before attaching the updated entity
            _context.Entry(existingNotification).State = EntityState.Detached;
            _context.Entry(notification).State = EntityState.Modified;

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
        // Delete an Notification
        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
                return false;

            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
        //filtre par date
        // Filtrer les notifications par date
        public async Task<List<Notification>> GetNotificationByDateAsync(DateTime date)
        {
            return await _context.Notification
                .Where(n => n.DateNotification.Date == date.Date)
                .ToListAsync();
        }

    }
}
