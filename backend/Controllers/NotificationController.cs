using System;

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
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Get all Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification()
        {
            return Ok(await _notificationService.GetAllNotificationAsync());
        }

        // Get an Notification by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null) return NotFound();
            return Ok(notification);
        }

        // Create an Notification
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
        {
            try
            {
                var createdNotification = await _notificationService.CreateNotificationAsync(notification);
                return CreatedAtAction(nameof(GetNotification), new { id = createdNotification.Id }, createdNotification);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update an Notification
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, Notification notification)
        {
            if (id != notification.Id) return BadRequest("The Notification ID does not match.");

            var result = await _notificationService.UpdateNotificationAsync(notification);

            if (!result) return NotFound("The Notification was not found or could not be updated.");

            // Return a success message along with a 200 OK status
            return Ok(new { message = "Appointment updated successfully." });
        }


        // Delete an appointment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var result = await _notificationService.DeleteNotificationAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // Filtrer par date (format attendu: yyyy-MM-dd)
        [HttpGet("date/{DateNotification}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationByDate(DateTime DateNotification)
        {
            var notification = await _notificationService.GetNotificationByDateAsync(DateNotification); // Utiliser DateNotification ici
            if (notification == null || notification.Count == 0)
                return NotFound("Aucun rendez-vous trouvé pour cette date.");
            return Ok(notification);
        }

    }
}
