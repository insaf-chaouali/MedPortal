import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthentificationService } from '../services/authentification.service';
import { CommonModule } from '@angular/common';  
import { FormsModule } from '@angular/forms';  

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule]  
})
export class LoginComponent {
  email = '';
  password = '';
  rememberMe = false;
  errorMessage = '';

  constructor(private authService: AuthentificationService, private router: Router) {}

  onSubmit() {
    this.authService.login({ login: this.email, password: this.password }).subscribe({
      next: (res) => {
        // Stockage dans sessionStorage
        sessionStorage.setItem('token', res.token);
        sessionStorage.setItem('userId', res.nameIdentifier);
        sessionStorage.setItem('role', res.role.toString());


        // Redirection selon le rôle
        if (res.role === 0) {
          this.router.navigate(['/home-patient']);
        } else if (res.role === 1) {
          this.router.navigate(['/home-medecin']);
        } else if (res.role === 2) {
          this.router.navigate(['/register-medecin']);
        } else {
          this.router.navigate(['/home']);
        }
      },
      error: (err) => {
        this.errorMessage = err.message || 'Erreur de connexion.';
      }
    });
  }
  contactAdmin() {
    alert("Si vous êtes médecin, contactez l'administrateur.\nSi vous êtes patient, contactez votre médecin.");
  }
  logout() {
    sessionStorage.clear(); // ou removeItem('token') etc.
    this.router.navigate(['/login']); // Redirection vers la page de login
  }
}
