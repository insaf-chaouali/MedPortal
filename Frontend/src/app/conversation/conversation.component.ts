import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';
import { FilterPipe } from '../filter.pipe';

@Component({
  selector: 'app-conversation',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, FilterPipe],
  templateUrl: './conversation.component.html',
  styleUrls: ['./conversation.component.css']
})
export class ConversationComponent implements OnInit {
  patients: Utilisateur[] = [];
  searchText: string = '';
  currentMedecin: Utilisateur | null = null; 
  medecinId!: string | null;



  constructor(
    private utilisateurService: UtilisateurService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const idMedecinString = sessionStorage.getItem('userId');
    const idMedecin = idMedecinString ? Number(idMedecinString) : null;
    this.fetchCurrentMedecin();


    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs) => {
        this.patients = utilisateurs.filter(p => p.ajoutePar === idMedecin && p.role === Role.Patient);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des patients :', err);
      }
    });
  }

  contacterPatient(patientId: number): void {
    this.router.navigate(['/message'], { queryParams: { patientId } });
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
   logout(event: Event) {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
