import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-medecin',
  templateUrl: './edit-medecin.component.html',
  styleUrls: ['./edit-medecin.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]
})
export class EditMedecinComponent {
  userId: number | null = null;
  newPassword = '';
  isSubmitting = false;
  message = '';
  isSuccess = false;
  formSubmitted = false;

  constructor(private http: HttpClient, private router: Router) {}

  showUserIdError(): boolean {
    return this.formSubmitted && !this.userId;
  }

  showPasswordError(): boolean {
    return this.formSubmitted && (!this.newPassword || this.newPassword.length < 8);
  }

  onSubmit(passwordForm: NgForm) {
    this.formSubmitted = true;
    
    if (passwordForm.invalid) {
      this.message = 'Veuillez corriger les erreurs dans le formulaire';
      this.isSuccess = false;
      return;
    }

    this.isSubmitting = true;
    this.message = '';

    this.http.put(
      `http://localhost:5127/api/utilisateur/edit-password/${this.userId}`,
      { nouveauPassword: this.newPassword },
      { responseType: 'text' }
    ).subscribe({
      next: (response: string) => {
        this.handleSuccessResponse(response);
      },
      error: (err: HttpErrorResponse) => {
        this.handleErrorResponse(err);
      }
    });
  }

  private handleSuccessResponse(response: string) {
    this.isSuccess = true;
    this.isSubmitting = false;

    try {
      const jsonResponse = JSON.parse(response);
      this.message = jsonResponse.message || 'Mot de passe modifié avec succès';
    } catch {
      this.message = response || 'Mot de passe modifié avec succès';
    }

    this.resetForm();

    // ✅ Redirection vers tableau-medecin après un court délai
    setTimeout(() => {
      this.router.navigate(['/tableau-medecin']);
    }, 1500);
  }

  private handleErrorResponse(error: HttpErrorResponse) {
    this.isSuccess = false;
    this.isSubmitting = false;

    if (error.error instanceof ErrorEvent) {
      this.message = 'Erreur réseau: ' + error.error.message;
    } else {
      try {
        const errorObj = typeof error.error === 'string' ? JSON.parse(error.error) : error.error;
        this.message = errorObj?.message || error.error || 'Erreur lors de la modification';
      } catch {
        this.message = error.error || 'Erreur lors de la modification';
      }
    }
  }

  private resetForm() {
    this.newPassword = '';
    this.formSubmitted = false;
  }
}
