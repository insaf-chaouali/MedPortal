import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Notification {
  id: number;
  titre: string;
  contenu: string;
  dateNotification: string; // Date ISO string
  sender?: number;          // ID du médecin
  reciver?: number; 
  // Pas besoin d'ajouter "patient" ou "medecin" côté frontend sauf si tu veux les afficher
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private apiUrl = 'http://localhost:5127/api/Notification'; // à adapter selon ton backend

  constructor(private http: HttpClient) {}

  // Obtenir toutes les notifications
  getAllNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(this.apiUrl);
  }

  // Obtenir une notification par ID
  getNotificationById(id: number): Observable<Notification> {
    return this.http.get<Notification>(`${this.apiUrl}/${id}`);
  }

  // Créer une notification
  createNotification(notification: Notification): Observable<Notification> {
    return this.http.post<Notification>(this.apiUrl, notification);
  }

  // Modifier une notification
  updateNotification(id: number, notification: Notification): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, notification);
  }

  // Supprimer une notification
  deleteNotification(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  // Obtenir les notifications par date (yyyy-MM-dd)
  getNotificationsByDate(date: string): Observable<Notification[]> {
    return this.http.get<Notification[]>(`${this.apiUrl}/date/${date}`);
  }
}
