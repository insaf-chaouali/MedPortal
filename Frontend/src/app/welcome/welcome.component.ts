import { Component } from '@angular/core';
import { NgModule } from '@angular/core';


@Component({
  selector: 'app-welcome',
  imports: [],
  templateUrl: './welcome.component.html',
  styleUrl: './welcome.component.css'
})
export class WelcomeComponent {
  email: string = '';

  services = [
    {
      name: 'Consultation Générale',
      price: 'À partir de 30€',
      description: 'Consultez nos médecins généralistes pour tout besoin de santé courant.'
    },
    {
      name: 'Téléconsultation',
      price: 'À partir de 20€',
      description: 'Consultez un médecin à distance, depuis chez vous.'
    },
    {
      name: 'Suivi Nutritionnel',
      price: 'À partir de 40€',
      description: 'Bénéficiez d’un accompagnement personnalisé avec nos nutritionnistes.'
    }
  ];

  onSubmit() {
    alert(`Merci ! Vous êtes inscrit(e) avec : ${this.email}`);
    this.email = '';
  }
}

