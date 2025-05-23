import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';
import { MedecinService } from '../services/medecin.service';
import { FilterPipe } from '../filter.pipe';

@Component({
  selector: 'app-tableau-patient',
  standalone: true,

  imports: [CommonModule, ReactiveFormsModule,RouterModule, FilterPipe,FormsModule],

  templateUrl: './tableau-patient.component.html',
  styleUrls: ['./tableau-patient.component.css']
})
export class TableauPatientComponent implements OnInit {
  patients: Utilisateur[] = [];
  searchText: string = '';
  currentMedecin: Utilisateur | null = null; 
  medecinId: string | null = null;

  constructor(
    private utilisateurService: UtilisateurService,
    private medecinService: MedecinService, 
    private router: Router
  ) {}

    ngOnInit(): void {
      const idMedecinString = sessionStorage.getItem('userId');
    const idMedecin = idMedecinString ? Number(idMedecinString) : null;
      this.fetchCurrentMedecin();
      this.utilisateurService.getAllUsers().subscribe({
        next: (utilisateurs) => {
        this.patients = utilisateurs.filter(p => p.ajoutePar === idMedecin);
        },
        error: (err) => {
          console.error('Erreur lors du chargement des utilisateurs :', err);
        }
      });
    }
    fetchCurrentMedecin(): void {
    this.medecinId = sessionStorage.getItem('userId');
    if (!this.medecinId) return;
    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs: Utilisateur[]) => {
        const id = Number(this.medecinId);
        this.currentMedecin = utilisateurs.find(u => u.id === id) || null;
        if (this.currentMedecin) {
          console.log('Patient connecté :', this.currentMedecin);
        } else {
          console.warn('Utilisateur non trouvé pour ID :', id);
        }
      },
      error: (err) => {
        console.error('Erreur lors de la récupération des utilisateurs :', err);
      }
    });
  }
    
    // Fonction d'édition
  editPatient(patient: any): void {
    this.router.navigate(['/edit-patient', patient.id]);
  }
    
    
    
  voirDossier(patientId?: number) {
    if (patientId !== undefined) {
      this.router.navigate(['/dossier-medical', patientId]);
    } else {
      console.warn('ID patient non défini, navigation annulée.');
    }
  }
  
  logout(event: Event) {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
  
}
