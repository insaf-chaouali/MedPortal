using System;
using Microsoft.AspNetCore.Mvc;
using projet_1.Models;
using projet_1.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace projet_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly UtilisateurService _utilisateurService;

        public UtilisateurController(UtilisateurService utilisateurService)
        {
            _utilisateurService = utilisateurService;
        }

        // POST: api/Utilisateur/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Vérifier l'utilisateur qui ajoute
                var utilisateurAjoutant = await _utilisateurService.GetByIdAsync(request.AjoutePar);
                if (utilisateurAjoutant == null)
                {
                    return Unauthorized(new { message = "Utilisateur ajoutant non trouvé." });
                }

                // Déterminer le rôle à attribuer
                Role roleAttribue;
                if (utilisateurAjoutant.Role == Role.Admin)
                {
                    roleAttribue = Role.Médecin;
                }
                else if (utilisateurAjoutant.Role == Role.Médecin)
                {
                    roleAttribue = Role.Patient;
                }
                else
                {
                    return Forbid("Vous n'avez pas l'autorisation d'ajouter un utilisateur.");
                }

                var nouvelUtilisateur = new Utilisateur
                {
                    Login = request.Email,
                    Password = "", // sera hashé dans CreateAsync
                    Email = request.Email,
                    LastName = request.LastName,
                    FirstName = request.FirstName,
                    BirthDate = request.BirthDate,
                    Gender = request.Gender,
                    Phone = request.Phone,
                    Address = request.Address,
                    City = request.City,
                    PostalCode = request.PostalCode,
                    Role = roleAttribue,
                    DateCreation = DateTime.Now,
                    Specialite = request.Specialite,// <-- AJOUT ICI
                    AjoutePar = request.AjoutePar
                };

                await _utilisateurService.CreateAsync(nouvelUtilisateur, request.Password);

                return CreatedAtAction(nameof(GetById), new { id = nouvelUtilisateur.Id }, new
                {
                    message = "Utilisateur enregistré avec succès",
                    utilisateur = new
                    {
                        nouvelUtilisateur.Id,
                        nouvelUtilisateur.Email,
                        nouvelUtilisateur.LastName,
                        nouvelUtilisateur.FirstName,
                        nouvelUtilisateur.Role,
                        nouvelUtilisateur.AjoutePar
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Une erreur est survenue", error = ex.Message });
            }
        }

        // GET: api/Utilisateur
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var utilisateurs = await _utilisateurService.GetAllAsync();
                return Ok(utilisateurs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        // GET: api/Utilisateur/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var utilisateur = await _utilisateurService.GetByIdAsync(id);
                if (utilisateur == null)
                {
                    return NotFound(new { message = "Utilisateur non trouvé" });
                }
                return Ok(utilisateur);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }

        // PUT: api/Utilisateur/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUtilisateur(int id, Utilisateur utilisateur)
        {
            if (id != utilisateur.Id)
            {
                return BadRequest(new { message = "Identifiants incohérents" });
            }

            await _utilisateurService.UpdateAsync(utilisateur);
            return NoContent();
        }

        // DELETE: api/Utilisateur/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            await _utilisateurService.DeleteAsync(id);
            return NoContent();
        }
        [HttpPut("edit-password/{id}")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest request)
        {
            var result = await _utilisateurService.ResetPasswordAsync(id, request.NouveauPassword);
            if (!result)
                return NotFound("Utilisateur non trouvé");

            return Ok("Mot de passe réinitialisé avec succès");
        }

    }
}

