import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-medecin',
  templateUrl: './home-medecin.component.html',
  styleUrls: ['./home-medecin.component.css'],
  standalone: true,
  imports: [CommonModule, RouterModule]
})
export class HomeMedecinComponent {
  logoPath = './assets/images/logo.png';
  
  constructor(private router: Router) {}

  // Méthode pour la navigation programmatique
  allerVersDashboard() {
    this.router.navigate(['/register']);
  }
}
