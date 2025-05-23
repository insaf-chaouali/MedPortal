import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminService, Utilisateur, Role } from '../services/admin-service';

@Component({
  selector: 'app-tableau-medecin',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './tableau-medecin.component.html',
  styleUrls: ['./tableau-medecin.component.css']
})
export class TableauMedecinComponent implements OnInit {
  medecins: Utilisateur[] = [];

  constructor(
    private adminService: AdminService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const idAdminString = sessionStorage.getItem('userId');
    const idAdmin = idAdminString ? Number(idAdminString) : null;
  
    this.adminService.getAllUsers().subscribe({
      next: (utilisateurs) => {
        this.medecins = utilisateurs.filter(p => p.ajoutePar === idAdmin);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des utilisateurs :', err);
      }
    });
  }
  // Fonction d'édition
  editMedecin(medecin: any): void {
    this.router.navigate(['/edit-medecin', medecin.id]);
  }
  

  // Fonction de suppression
  deleteMedecin(medecin: Utilisateur): void {
    if (confirm(`Êtes-vous sûr de vouloir supprimer ${medecin.firstName} ${medecin.lastName} ?`)) {
      this.adminService.deleteUtilisateur(medecin.id!).subscribe({
        next: () => {
          // Supprimer du tableau local après succès côté back
          this.medecins = this.medecins.filter(m => m.id !== medecin.id);
          console.log('Médecin supprimé avec succès.');
        },
        error: (err) => {
          console.error('Erreur lors de la suppression :', err);
          alert("Échec de la suppression du médecin.");
        }
      });
    }
  }
    logout(event: Event) {
      event.preventDefault();
      sessionStorage.clear();
      this.router.navigate(['/login']);
    }
    
 


}
