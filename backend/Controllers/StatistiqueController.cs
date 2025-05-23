using System;
using Microsoft.AspNetCore.Mvc;
using projet_1.Models;
using projet_1.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace projet_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatistiqueController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatistiqueController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("medecins-par-jour")]
        public IActionResult GetMedecinsParJour()
        {
            var stats = _context.Utilisateurs
                .Where(u => u.Role == Role.Médecin)
                .GroupBy(u => u.DateCreation.Date) // Groupement par jour exact
                .Select(g => new
                {
                    Date = g.Key,
                    Nombre = g.Count()
                })
                .ToList() // On récupère les données de la DB ici
                .Select(g => new Statistique
                {
                    Jours = g.Date.ToString("dd MMM yyyy"), // Nom clair, format lisible
                    Nombre = g.Nombre
                })
                .OrderBy(s => DateTime.ParseExact(s.Jours, "dd MMM yyyy", null))
                .ToList();

            return Ok(stats);
        }
        [HttpGet("medecins-par-specialite")]
        public IActionResult GetMedecinsParSpecialite()
        {
            var stats = _context.Utilisateurs
                .Where(u => u.Role == Role.Médecin && !string.IsNullOrEmpty(u.Specialite))
                .GroupBy(u => u.Specialite)
                .Select(g => new
                {
                    Specialite = g.Key,
                    Nombre = g.Count()
                })
                .ToList();

            return Ok(stats);
        }


    }
}
