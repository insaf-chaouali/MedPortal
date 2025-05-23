using System;
using System.Collections.Generic;
using projet_1.Data;
using Newtonsoft.Json;


namespace projet_1.Models

{

    public class Notification
    {
        public int Id { get; set; }
        public string Titre { get; set; } 
        public string Contenu { get; set; }
        public DateTime DateNotification { get; set; }

        public int? Reciver { get; set; }
        [JsonIgnore]
        public virtual Utilisateur? Patient { get; set; }

        public int? Sender { get; set; }
        [JsonIgnore]
        public virtual Utilisateur? Medecin { get; set; }
    }

}
