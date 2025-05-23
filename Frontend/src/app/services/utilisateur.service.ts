import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export enum Role {
  Patient = 0,
  Medecin = 1,
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
  specialite?: string;
  ajoutePar?: number;
}

@Injectable({
  providedIn: 'root'
})
export class UtilisateurService {

  private apiUrl = 'http://localhost:5127/api/Utilisateur';

  constructor(private http: HttpClient) { }

  registerPatient(patientData: Utilisateur): Observable<any> {
    const registerData = {
      ...patientData,
      birthDate: new Date(patientData.birthDate).toISOString(),
      role: Role.Patient, 
      login: patientData.email 
    };

    return this.http.post<any>(`${this.apiUrl}/register`, registerData)
      .pipe(
        catchError(this.handleError)
      );
  }

  register(utilisateur: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, utilisateur, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      catchError(error => {
        console.error('Erreur de connexion:', error);
        if (error.status === 0) {
          return throwError(() => ({
            message: 'Impossible de se connecter au serveur. Veuillez vérifier que le serveur est en cours d\'exécution.'
          }));
        }
        return throwError(() => error.error || {
          message: 'Une erreur est survenue lors de la communication avec le serveur'
        });
      })
    );
  }
loadAndStoreFullUser(): Observable<any> {
  const userIdString = sessionStorage.getItem('userId');
  const userId = userIdString ? Number(userIdString) : null;

  if (!userId) {
    return throwError(() => new Error('Aucun ID utilisateur trouvé dans sessionStorage'));
  }

  return this.http.get<any[]>('http://localhost:5127/api/utilisateur').pipe(
    map(users => {
      const currentUser = users.find(u => u.id === userId);
      if (currentUser) {
        localStorage.setItem('user', JSON.stringify(currentUser));
        return currentUser;
      } else {
        throw new Error('Utilisateur non trouvé dans la base');
      }
    }),
    catchError(err => {
      console.error('Erreur lors du chargement de l’utilisateur :', err);
      return throwError(() => new Error('Échec de récupération de l\'utilisateur'));
    })
  );
}
  getAllUsers(): Observable<Utilisateur[]> {
    return this.http.get<Utilisateur[]>(`${this.apiUrl}`)
      .pipe(
        catchError(this.handleError)
      );
  }
  updateUtilisateur(id: number, utilisateur: Utilisateur): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, utilisateur, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      catchError(this.handleError)
    );
  }
  deleteUtilisateur(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    }).pipe(
      catchError(this.handleError)
    );
  }
  getUserById(id: number): Observable<Utilisateur> {
  return this.http.get<Utilisateur>(`${this.apiUrl}/${id}`);
}

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Une erreur est survenue';
    if (error.error instanceof ErrorEvent) {
      // Erreur côté client
      errorMessage = `Erreur: ${error.error.message}`;
    } else {
      // Erreur côté serveur
      errorMessage = `Code d'erreur: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
