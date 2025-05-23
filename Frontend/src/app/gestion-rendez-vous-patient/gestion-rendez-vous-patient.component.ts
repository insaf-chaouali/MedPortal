import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import type { CalendarOptions, EventClickArg } from '@fullcalendar/core';
import { Router } from '@angular/router';
import { RendezVousService, RendezVous } from '../services/rendezvous.service';
import { AuthentificationService } from '../services/authentification.service';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';

export enum EtatRdv {
  EnAttente = 'en attente',
  Confirme = 'confirmé',
  Annule = 'annulé'
}

@Component({
  selector: 'app-gestion-rendez-vous-patient',
  templateUrl: './gestion-rendez-vous-patient.component.html',
  styleUrls: ['./gestion-rendez-vous-patient.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FullCalendarModule]
})
export class GestionRendezVousPatientComponent implements OnInit {

  rdvForm: FormGroup;
  rendezVousList: RendezVous[] = [];
  selectedEvent: RendezVous | null = null;
  logoPath = './assets/images/logo.png';
  patientId: string | null = null;
  medecins: Utilisateur[] = [];
  public EtatRdv = EtatRdv;
  currentPatient: Utilisateur | null = null;

  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    locale: 'fr',
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay'
    },
    height: 350,
    events: [],
    editable: false,
    selectable: false,
    selectMirror: false,
    dayMaxEvents: true,
    eventClick: this.handleEventClick.bind(this)
  };

  constructor(
    private fb: FormBuilder,
    private rdvService: RendezVousService,
    public authService: AuthentificationService,
    private utilisateurService: UtilisateurService,
    private router: Router

  ) {
    this.rdvForm = this.fb.group({
      date: ['', Validators.required],
      titre: ['', [Validators.required, Validators.minLength(3)]],
      etat: [EtatRdv.EnAttente, Validators.required],
      descriptionRdv: [''],
      patientId: [null, Validators.required],
      medecinId: [null],
    });
  }

  ngOnInit(): void {
    this.patientId = sessionStorage.getItem('userId');
    if (this.patientId) {
      this.rdvForm.patchValue({ patientId: this.patientId });
      this.fetchCurrentPatient();

    }
    this.fetchRendezVousList();
    this.fetchMedecins();
  }

  onSubmit(): void {
    if (this.rdvForm.valid) {
      const formData = this.rdvForm.value;
      this.rdvService.create(formData).subscribe({
        next: (response: RendezVous) => {
          this.rendezVousList.push(response);
          this.updateCalendarEvents();
          this.rdvForm.reset({
            etat: EtatRdv.EnAttente,
            patientId: this.patientId
          });
          alert('Rendez-vous ajouté avec succès');
        },
        error: (error: any) => {
          console.error('Erreur lors de l\'ajout du rendez-vous :', error);
        }
      });
    }
  }

  fetchRendezVousList(): void {
    if (!this.patientId) return;

    this.rdvService.getAll().subscribe({
      next: (data: RendezVous[]) => {
        const parsedUserId = Number(this.patientId);
        this.rendezVousList = data.filter(rdv => Number(rdv.patientId) === parsedUserId);
        this.updateCalendarEvents();
      },
      error: (error: any) => {
        console.error('Erreur lors de la récupération des rendez-vous :', error);
      }
    });
  }

  fetchMedecins(): void {
    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs) => {
        this.medecins = utilisateurs.filter(u => u.role === Role.Medecin);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des médecins :', err);
      }
    });
  }

  updateCalendarEvents(): void {
    this.calendarOptions.events = this.rendezVousList.map(rdv => ({
      id: rdv.id?.toString(),
      title: rdv.titre,
      date: rdv.date
    }));
  }

  updateEtatRendezVous(rdv: RendezVous, nouvelEtat: EtatRdv): void {
    const updated = { ...rdv, etat: nouvelEtat };
    this.rdvService.update(updated).subscribe({
      next: () => {
        rdv.etat = nouvelEtat;
        this.updateCalendarEvents();
      },
      error: (error) => {
        console.error(`Erreur lors de la mise à jour de l'état :`, error);
      }
    });
  }

  confirmerRendezVous(rdv: RendezVous): void {
    const updatedRdv: RendezVous = { ...rdv, etat: EtatRdv.Confirme };
    this.rdvService.update(updatedRdv).subscribe({
      next: () => {
        rdv.etat = EtatRdv.Confirme;
        this.updateCalendarEvents();
      },
      error: err => console.error('Erreur confirmation:', err)
    });
  }

  annulerRendezVous(rdv: RendezVous): void {
    const updatedRdv = { ...rdv, etat: EtatRdv.Annule };
    this.rdvService.update(updatedRdv).subscribe({
      next: () => {
        rdv.etat = EtatRdv.Annule;
        this.updateCalendarEvents();
      },
      error: err => {
        console.error('Erreur lors de l\'annulation :', err);
      }
    });
  }

  handleEventClick(arg: EventClickArg): void {
    const eventId = arg.event.id;
    this.selectedEvent = this.rendezVousList.find(rdv => rdv.id?.toString() === eventId) || null;

    const modalElement = document.getElementById('eventModal');
    if (modalElement && (window as any).bootstrap?.Modal) {
      const modal = new (window as any).bootstrap.Modal(modalElement);
      modal.show();
    }
  }

  getNomMedecin(medecinId: number | null): string {
    const medecin = this.medecins.find(m => m.id === medecinId);
    return medecin ? `${medecin.firstName} ${medecin.lastName}` : 'Médecin inconnu';
  }

  fetchCurrentPatient(): void {
    if (!this.patientId) return;

    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs: Utilisateur[]) => {
        const id = Number(this.patientId);
        this.currentPatient = utilisateurs.find(u => u.id === id) || null;
        if (this.currentPatient) {
          console.log('Patient connecté :', this.currentPatient);
        } else {
          console.warn('Utilisateur non trouvé pour ID :', id);
        }
      },
      error: (err) => {
        console.error('Erreur lors de la récupération des utilisateurs :', err);
      }
    });
  }

  logout(event: Event): void {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
