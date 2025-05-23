import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-patient',
  templateUrl: './home-patient.component.html',
  styleUrls: ['./home-patient.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class HomePatientComponent {
  logoPath = './assets/images/logo.png';
  
  constructor(private router: Router) {}

  // Méthode pour la navigation programmatique
  allerVersDashboard() {
    this.router.navigate(['/gestion-rendez-vous-patient']);
  }
}
