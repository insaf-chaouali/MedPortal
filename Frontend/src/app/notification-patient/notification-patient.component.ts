import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../services/notification.service';
import { Router } from '@angular/router';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';

@Component({
  selector: 'app-notification-patient',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification-patient.component.html',
  styleUrls: ['./notification-patient.component.css']
})
export class NotificationPatientComponent implements OnInit {
  notifications: Notification[] = [];
  currentPatient: Utilisateur | null = null;

  constructor(
    private notificationService: NotificationService,
    private utilisateurService: UtilisateurService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const patientIdString = sessionStorage.getItem('userId');
    const patientId = patientIdString ? Number(patientIdString) : null;

    this.fetchCurrentPatient();

    if (!patientId) {
      console.error('Patient non connecté');
      return;
    }

    this.notificationService.getAllNotifications().subscribe({
      next: (allNotifications) => {
        this.notifications = allNotifications
          .filter(n => n.reciver === patientId)
          .sort((a, b) => 
            new Date(b.dateNotification).getTime() - new Date(a.dateNotification).getTime()
          ); // tri décroissant par date
      },
      error: (err) => {
        console.error('Erreur lors du chargement des notifications', err);
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

  logout(event: Event): void {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
