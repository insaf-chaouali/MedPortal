using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using projet_1.Services;
    using projet_1.Models;

namespace projet_1.Models

{
    public class DossierMedical
    {
        [Key]
        public int Id { get; set; }

        public string? Taille { get; set; }

        public string? Poids { get; set; }

        public string? GroupeSanguin { get; set; }

        public string? Antecedents { get; set; }

        public string? Traitements { get; set; }

        public string? Allergies { get; set; }

        public string? Observations { get; set; }

        public int PatientId { get; set; }

        [JsonIgnore]
        public virtual Utilisateur? Patient { get; set; }

        public int MedecinId { get; set; }

        [JsonIgnore]
        public virtual Utilisateur? Medecin { get; set; }
    }


}
