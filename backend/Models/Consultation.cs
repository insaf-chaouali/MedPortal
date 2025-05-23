using System;
using System.Collections.Generic;

namespace projet_1.Models
{

    public class Consultation
    {
        public int Id { get; set; }
        public DateTime DateConsultation { get; set; }
        public required string Historique { get; set; }

        // Relationships
        public int UtilisateurId { get; set; }
        public required virtual Utilisateur Utilisateur { get; set; }
        public int DossierMedicalId { get; set; }
        public required virtual DossierMedical DossierMedical { get; set; }
        public required virtual ICollection<Traitement> Traitements { get; set; } = new List<Traitement>();
    }
}
