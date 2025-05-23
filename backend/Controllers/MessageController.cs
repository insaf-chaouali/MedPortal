using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projet_1.Data;
using projet_1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace projet_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/messages/conversation?user1=1&user2=2
        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation(int user1, int user2)
        {
            var messages = await _context.Messages
                .Where(m =>
                    (m.EnvoyeurId == user1 && m.ReceveurId == user2) ||
                    (m.EnvoyeurId == user2 && m.ReceveurId == user1))
                .OrderBy(m => m.DateEnvoi)
                .ToListAsync();

            return Ok(messages);
        }

        // POST: api/messages/send
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            message.DateEnvoi = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return Ok(message);
        }
    }
}
