using Microsoft.AspNetCore.SignalR;
using projet_1.Data;
using projet_1.Models;
using System;
using System.Threading.Tasks;

namespace projet_1.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        // Envoi de message privé
        public async Task SendMessage(int senderId, int receiverId, string messageContent)
        {
            var message = new Message
            {
                Contenu = messageContent,
                DateEnvoi = DateTime.Now,
                EnvoyeurId = senderId,
                ReceveurId = receiverId
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Envoi du message au receveur
            await Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", senderId, messageContent, message.DateEnvoi);
        }

        // Optionnel : Gérer la déconnexion
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Traitement optionnel lors de la déconnexion
            await base.OnDisconnectedAsync(exception);
        }
    }
}
