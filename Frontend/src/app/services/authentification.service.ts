import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthentificationService {
  private apiUrl = 'http://localhost:5127/api/auth';  // Updated apiUrl to remove the duplicate '/login'

  constructor(private http: HttpClient) {}

  getCurrentUser(): any {
    // Ensure the code runs only in the browser and check localStorage for user data
    if (typeof window !== 'undefined' && localStorage.getItem('user')) {
      return JSON.parse(localStorage.getItem('user') || '{}');
    }
    return null;
  }

  login(credentials: { login: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      map(response => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          localStorage.setItem('role', response.role.toString());  // Ensure role is saved as string
          localStorage.setItem('user', JSON.stringify(response));  // Store the full user data
        }
        return response;
      }),
      catchError((error: HttpErrorResponse) => {
        const msg = error.status === 401 ? 'Email ou mot de passe incorrect' : 'Erreur de connexion';
        return throwError(() => new Error(msg));
      })
    );
  }

  getRole(): number {
    return parseInt(localStorage.getItem('role') || '-1');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
  

  logout(): void {
    localStorage.clear();
  }
}
