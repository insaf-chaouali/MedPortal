using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using projet_1.Data;
using projet_1.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace projet_1.Services
{
    public class UtilisateurService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<Utilisateur> _passwordHasher;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;

        public UtilisateurService(
            ApplicationDbContext context,
            IConfiguration configuration,
            IPasswordHasher<Utilisateur> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;

            _smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            _smtpUsername = _configuration["EmailSettings:Username"] ?? "";
            _smtpPassword = _configuration["EmailSettings:Password"] ?? "";
            _fromEmail = _configuration["EmailSettings:FromEmail"] ?? "";
        }

        public async Task SendConfirmationEmailAsync(string email, string login, string password)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("MedPortal", _fromEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Bienvenue sur MedPortal - Vos informations de connexion";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                            <h2 style='color: #2c3e50;'>Bienvenue sur MedPortal</h2>
                            <p>Cher patient,</p>
                            <p>Votre compte a été créé avec succès. Voici vos informations de connexion :</p>
                            <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <p><strong>Login :</strong> {login}</p>
                                <p><strong>Mot de passe :</strong> {password}</p>
                            </div>
                            <p>Pour votre sécurité, nous vous recommandons de changer votre mot de passe après votre première connexion.</p>
                            <p>Cordialement,<br>L'équipe MedPortal</p>
                        </div>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'email : {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception : {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task SendPasswordChangedEmailAsync(string email, string login, string password)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("MedPortal", _fromEmail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "MedPortal - Modification de votre mot de passe";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                            <h2 style='color: #2c3e50;'>Modification de mot de passe</h2>
                            <p>Bonjour,</p>
                            <p>Votre mot de passe a été modifié avec succès. Voici vos nouvelles informations de connexion :</p>
                            <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                <p><strong>Login :</strong> {login}</p>
                                <p><strong>Nouveau mot de passe :</strong> {password}</p>
                            </div>
                            <p>Si vous n'avez pas demandé ce changement, veuillez contacter immédiatement notre support.</p>
                            <p>Cordialement,<br>L'équipe MedPortal</p>
                        </div>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpUsername, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'envoi de l'e-mail : {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner exception : {ex.InnerException.Message}");
                throw;
            }
        }

        public async Task CreateAsync(Utilisateur utilisateur, string motDePasseNonCrypte)
        {
            if (await EmailExistsAsync(utilisateur.Email))
                throw new InvalidOperationException("Cet email est déjà utilisé.");

            if (await LoginExistsAsync(utilisateur.Login))
                throw new InvalidOperationException("Ce login est déjà utilisé.");

            if (utilisateur.Role != Role.Médecin && utilisateur.Role != Role.Patient)
                throw new ArgumentException("Le rôle spécifié n'est pas valide.");

            utilisateur.Password = _passwordHasher.HashPassword(utilisateur, motDePasseNonCrypte);

            await _context.Utilisateurs.AddAsync(utilisateur);
            await _context.SaveChangesAsync();

            await SendConfirmationEmailAsync(utilisateur.Email, utilisateur.Login, motDePasseNonCrypte);
        }

        public async Task<bool> EmailExistsAsync(string email) =>
            await _context.Utilisateurs.AnyAsync(u => u.Email == email);

        public async Task<bool> LoginExistsAsync(string login) =>
            await _context.Utilisateurs.AnyAsync(u => u.Login == login);

        public async Task<IEnumerable<Utilisateur>> GetAllAsync() =>
            await _context.Utilisateurs.ToListAsync();

        public async Task<Utilisateur> GetByIdAsync(int id) =>
            await _context.Utilisateurs.FindAsync(id);

        public async Task UpdateAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Update(utilisateur);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ResetPasswordAsync(int userId, string nouveauPassword)
        {
            var utilisateur = await _context.Utilisateurs.FindAsync(userId);
            if (utilisateur == null) return false;

            utilisateur.Password = _passwordHasher.HashPassword(utilisateur, nouveauPassword);
            await _context.SaveChangesAsync();
            await SendPasswordChangedEmailAsync(utilisateur.Email, utilisateur.Login, nouveauPassword);
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var utilisateur = await GetByIdAsync(id);
            if (utilisateur != null)
            {
                _context.Utilisateurs.Remove(utilisateur);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Utilisateur>> GetPatientsByMedecinAsync(int idMedecin) =>
            await _context.Utilisateurs
                .Where(u => u.Role == Role.Patient && u.AjoutePar == idMedecin)
                .ToListAsync();

        public async Task<List<Utilisateur>> GetMedecinsByAdminAsync(int idAdmin) =>
            await _context.Utilisateurs
                .Where(u => u.Role == Role.Médecin && u.AjoutePar == idAdmin)
                .ToListAsync();

        /*public async Task<DossierMedical> GetDossierMedicalAsync(int patientId) =>
            await _dossierMedicalService.GetDossierByPatientIdAsync(patientId);
    }*/
    }
}
