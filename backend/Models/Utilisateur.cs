using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projet_1.Models;

public class Utilisateur
{
    public Utilisateur()
    {
        RendezVousPatient = new List<RendezVous>();
        RendezVousMedecin = new List<RendezVous>();
        DossiersMedicaux = new List<DossierMedical>();
        Consultations = new List<Consultation>();
        Notifications = new List<Notification>();
        MessagesEnvoyes = new List<Message>();
        MessagesRecus = new List<Message>();
    }

    public int Id { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public Role Role { get; set; }
    public required string Email { get; set; }
    public required string LastName { get; set; }
    public required string FirstName { get; set; }
    public DateTime BirthDate { get; set; }
    public required string Gender { get; set; }
    public required string Phone { get; set; }
    public string? Address { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public string? Specialite { get; set; }
    public int? AjoutePar { get; set; }


    // Propriétés de navigation
    public virtual ICollection<DossierMedical> DossiersMedicaux { get; set; }
    public virtual ICollection<Consultation> Consultations { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<Message> MessagesEnvoyes { get; set; }
    public virtual ICollection<Message> MessagesRecus { get; set; }
    
    [InverseProperty("Patient")]
    public virtual ICollection<RendezVous> RendezVousPatient { get; set; }
    
    [InverseProperty("Medecin")]
    public virtual ICollection<RendezVous> RendezVousMedecin { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<DossierMedical> DossierMedicalPatient { get; set; }

    [InverseProperty("Medecin")]
    public virtual ICollection<DossierMedical> DossierMedicalMedecin { get; set; }

    [InverseProperty("Patient")]
    public virtual ICollection<Notification> NotificationPatient { get; set; }

    [InverseProperty("Medecin")]
    public virtual ICollection<Notification> NotificationMedecin { get; set; }

    // Propriétés de compatibilité
    public string Nom => LastName;
    public string Prenom => FirstName;
}