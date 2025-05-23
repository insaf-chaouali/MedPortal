import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotificationService, Notification } from '../services/notification.service';
import { Router } from '@angular/router';
import { UtilisateurService, Utilisateur } from '../services/utilisateur.service';

@Component({
  selector: 'app-notification',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  notifications: Notification[] = [];
  currentMedecin: Utilisateur | null = null;
  medecinId: number | null = null; 
  reciver: number | null = null;

  constructor(
    private notificationService: NotificationService,
    private utilisateurService: UtilisateurService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const medecinIdString = sessionStorage.getItem('userId');
    this.medecinId = medecinIdString ? Number(medecinIdString) : null;

    if (!this.medecinId) {
      console.error('Aucun médecin connecté.');
      return;
    }

    this.fetchCurrentMedecinAndNotifications(this.medecinId);
  }

  fetchCurrentMedecinAndNotifications(id: number): void {
  this.utilisateurService.getAllUsers().subscribe({
    next: (utilisateurs: Utilisateur[]) => {
      this.currentMedecin = utilisateurs.find(u => u.id === id) || null;

      if (this.currentMedecin) {
        console.log('Médecin connecté :', this.currentMedecin);

        // Charger uniquement les notifications où reciver === medecinId
        this.notificationService.getAllNotifications().subscribe({
          next: (allNotifications) => {
            this.notifications = allNotifications
              .filter(n => n.reciver === id)
              .sort((a, b) =>
                new Date(b.dateNotification).getTime() - new Date(a.dateNotification).getTime()
              );
          },
          error: (err) => {
            console.error('Erreur lors du chargement des notifications', err);
          }
        });

      } else {
        console.warn(`Médecin non trouvé pour l'ID : ${id}`);
      }
    },
    error: (err) => {
      console.error('Erreur lors de la récupération des utilisateurs :', err);
    }
  });
}


  loadNotifications(): void {
  if (!this.medecinId) return;

  this.notificationService.getAllNotifications().subscribe({
    next: (allNotifications) => {
      this.notifications = allNotifications
        .filter(n => n.sender === this.medecinId) 
        .sort((a, b) =>
          new Date(b.dateNotification).getTime() - new Date(a.dateNotification).getTime()
        );
    },
    error: (err) => {
      console.error('Erreur lors du chargement des notifications', err);
    }
  });
}


  logout(event: Event): void {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
