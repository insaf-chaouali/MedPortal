using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace projet_1.Models
{
    public class RendezVous
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Titre { get; set; }
        public string Etat { get; set; }
        public string DescriptionRdv { get; set; }
        public int PatientId { get; set; }

        [JsonIgnore]
        public virtual Utilisateur? Patient { get; set; }  // Nullable here

        public int MedecinId { get; set; }

        [JsonIgnore]
        public virtual Utilisateur? Medecin { get; set; }  // Nullable here
    }
}
