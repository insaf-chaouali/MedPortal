using Microsoft.EntityFrameworkCore;
using projet_1.Models;

namespace projet_1.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<Traitement> Traitements { get; set; }
    public DbSet<Notification> Notification { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<RendezVous> RendezVous { get; set; }
    public DbSet<DossierMedical> DossierMedical { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Message - Envoyeur
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Envoyeur)
            .WithMany(u => u.MessagesEnvoyes)
            .HasForeignKey(m => m.EnvoyeurId)
            .OnDelete(DeleteBehavior.Restrict);

        // Message - Receveur
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receveur)
            .WithMany(u => u.MessagesRecus)
            .HasForeignKey(m => m.ReceveurId)
            .OnDelete(DeleteBehavior.Restrict);

        

        // Utilisateur - Consultation
        modelBuilder.Entity<Utilisateur>()
            .HasMany(u => u.Consultations)
            .WithOne(c => c.Utilisateur)
            .HasForeignKey(c => c.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        // Consultation - Traitement
        modelBuilder.Entity<Consultation>()
            .HasMany(c => c.Traitements)
            .WithOne(t => t.Consultation)
            .HasForeignKey(t => t.ConsultationId)
            .OnDelete(DeleteBehavior.Cascade);

        // ✔️ RendezVous - Patient
        modelBuilder.Entity<RendezVous>()
            .HasOne(r => r.Patient)
            .WithMany(u => u.RendezVousPatient)
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✔️ RendezVous - Medecin
        modelBuilder.Entity<RendezVous>()
            .HasOne(r => r.Medecin)
            .WithMany(u => u.RendezVousMedecin)
            .HasForeignKey(r => r.MedecinId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✔️ DossierMedical - Patient
        modelBuilder.Entity<DossierMedical>()
            .HasOne(r => r.Patient)
            .WithMany(u => u.DossierMedicalPatient)
            .HasForeignKey(r => r.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✔️ DossierMedical - Medecin
        modelBuilder.Entity<DossierMedical>()
            .HasOne(r => r.Medecin)
            .WithMany(u => u.DossierMedicalMedecin)
            .HasForeignKey(r => r.MedecinId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✔️ Notification - Patient
        modelBuilder.Entity<Notification>()
            .HasOne(r => r.Patient)
            .WithMany(u => u.NotificationPatient)
            .HasForeignKey(r => r.Reciver)
            .OnDelete(DeleteBehavior.Restrict);

        // ✔️ Notification - Medecin
        modelBuilder.Entity<Notification>()
            .HasOne(r => r.Medecin)
            .WithMany(u => u.NotificationMedecin)
            .HasForeignKey(r => r.Sender)
            .OnDelete(DeleteBehavior.Restrict);

    }


}
