import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Message {
  id: number;
  envoyeurId: number;
  receveurId: number;
  contenu: string;
  dateEnvoi: string; // ISO 8601 format
}

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  private apiUrl = 'http://localhost:5127/api/messages';

  constructor(private http: HttpClient) {}

  // 🔹 Récupérer la conversation entre deux utilisateurs
  getConversation(user1Id: number, user2Id: number): Observable<Message[]> {
    const params = new HttpParams()
      .set('user1', user1Id.toString())
      .set('user2', user2Id.toString());

    return this.http.get<Message[]>(`${this.apiUrl}/conversation`, { params });
  }

  // 🔹 Envoyer un message
  sendMessage(message: Omit<Message, 'id' | 'dateEnvoi'>): Observable<Message> {
    return this.http.post<Message>(`${this.apiUrl}/send`, message);
  }
}
