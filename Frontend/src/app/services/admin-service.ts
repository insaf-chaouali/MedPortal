import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export enum Role {
  Patient = 0,
  Médecin = 1,
  Admin = 2
}

export interface Utilisateur {
  id?: number;
  login: string;
  password: string;
  role: Role;
  email: string;
  lastName: string;
  firstName: string;
  birthDate: string;
  gender: string;
  phone: string;
  address?: string;
  city: string;
  postalCode: string;
  dateCreation?: string;
  specialite? : string;
  ajoutePar?: number;
}

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5127/api/Utilisateur'; // Remplace cette URL par l'URL de ton API
  
  constructor(private http: HttpClient) { }
  
  // Méthode pour enregistrer un médecin
  registerMédecin(medecinData: Utilisateur): Observable<any> {
    const registerData = {
      ...medecinData,
      birthDate: new Date(medecinData.birthDate).toISOString(),
      role: Role.Médecin,  // Définir le rôle comme Médecin
      login: medecinData.email // Utiliser l'email comme login
    };
    return this.http.post<any>(`${this.apiUrl}/register`, registerData)
      .pipe(catchError(this.handleError));
  }

  // Méthode pour mettre à jour le mot de passe d'un utilisateur
  updatePassword(userId: number, newPassword: string): Observable<any> {
    const body = { password: newPassword };  // Créer un objet avec la nouvelle valeur de mot de passe
    return this.http.put(`${this.apiUrl}/update-password/${userId}`, body, {
      headers: { 'Content-Type': 'application/json' }
    }).pipe(catchError(this.handleError));
  }

  // Méthode pour récupérer tous les utilisateurs
  getAllUsers(): Observable<Utilisateur[]> {
    return this.http.get<Utilisateur[]>(`${this.apiUrl}`)
      .pipe(catchError(this.handleError));
  }

  // Méthode pour supprimer un utilisateur
  deleteUtilisateur(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(catchError(this.handleError));
  }

  // Gestion des erreurs
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Une erreur est survenue';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Erreur: ${error.error.message}`;
    } else {
      errorMessage = `Code d'erreur: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
