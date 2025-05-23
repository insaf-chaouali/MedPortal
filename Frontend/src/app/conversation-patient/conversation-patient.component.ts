import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';
import { FilterPipe } from '../filter.pipe';

@Component({
  selector: 'app-conversation-patient',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, FilterPipe],
  templateUrl: './conversation-patient.component.html',
  styleUrls: ['./conversation-patient.component.css']
})
export class ConversationPatientComponent implements OnInit {
  patients: Utilisateur[] = [];
  searchText: string = '';
  currentPatient: Utilisateur | null = null;
  constructor(
    private utilisateurService: UtilisateurService,
    private router: Router
  ) {}

  ngOnInit(): void {
  const idPatientString = sessionStorage.getItem('userId');
  const idPatient = idPatientString ? Number(idPatientString) : null;
  this.fetchCurrentPatient();
  if (!idPatient) {
    console.error('Aucun patient connecté trouvé');
    return;
  }

  // Récupérer le patient connecté
  this.utilisateurService.getUserById(idPatient).subscribe({
    next: (patient) => {
      if (!patient) {
        console.error('Patient non trouvé');
        return;
      }

      console.log('Patient connecté:', patient);

      // Récupérer le médecin qui a ajouté ce patient (champ ajoutePar contient l'id médecin)
      if (!patient.ajoutePar) {
        console.error('Aucun médecin ajouté pour ce patient');
        return;
      }

      this.utilisateurService.getUserById(patient.ajoutePar).subscribe({
        next: (medecin) => {
          if (!medecin) {
            console.error('Médecin non trouvé');
            return;
          }

          console.log('Médecin qui a ajouté le patient:', medecin);

          // Ici, on met dans un tableau patients uniquement ce médecin pour affichage
          this.patients = [medecin];
        },
        error: (err) => {
          console.error('Erreur lors de la récupération du médecin :', err);
        }
      });
    },
    error: (err) => {
      console.error('Erreur lors de la récupération du patient :', err);
    }
  });
}
fetchCurrentPatient(): void {
  const patientId = sessionStorage.getItem('userId');
  if (!patientId) return;

  this.utilisateurService.getAllUsers().subscribe({
    next: (utilisateurs: Utilisateur[]) => {
      const id = Number(patientId);
      this.currentPatient = utilisateurs.find(u => u.id === id) || null;
      if (this.currentPatient) {
        console.log('Patient connecté :', this.currentPatient);
      } else {
        console.warn('Utilisateur non trouvé pour ID :', id);
      }
    },
    error: (err) => {
      console.error('Erreur lors de la récupération des utilisateurs :', err);
    }
  });
}


  contacterPatient(patientId: number): void {
    this.router.navigate(['/message'], { queryParams: { patientId } });
  }
   logout(event: Event) {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
