using System;
using System.Collections.Generic;

namespace projet_1.Models
{
    public class Traitement
    {
        public int Id { get; set; }
        public string NomTraitement { get; set; }
        public DateTime DateDebut { get; set; }
        public string Description { get; set; }

        // Relationships
        public int ConsultationId { get; set; }
        public virtual Consultation Consultation { get; set; }
    }
}
