using System;
using Newtonsoft.Json;

namespace projet_1.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Contenu { get; set; }
        public DateTime DateEnvoi { get; set; }

        public int? EnvoyeurId { get; set; }
        [JsonIgnore]
        public virtual Utilisateur? Envoyeur { get; set; }

        public int? ReceveurId { get; set; }
        [JsonIgnore]
        public virtual Utilisateur? Receveur { get; set; }
    }
}
