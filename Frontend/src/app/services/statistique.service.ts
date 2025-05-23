import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Statistique {
  jours: string;
  nombre: number;
}

@Injectable({
  providedIn: 'root'
})
export class StatistiqueService {
  private apiUrl = 'http://localhost:5127/api/statistique/medecins-par-jour'; // adapte selon ton port

  constructor(private http: HttpClient) {}

  getStats(): Observable<Statistique[]> {
    return this.http.get<Statistique[]>(this.apiUrl);
  }
  getStatsParSpecialite(): Observable<{ specialite: string, nombre: number }[]> {
    return this.http.get<{ specialite: string, nombre: number }[]>('http://localhost:5127/api/statistique/medecins-par-specialite');
  }
  
}
