import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import type { CalendarOptions, EventClickArg } from '@fullcalendar/core';
import { Router } from '@angular/router';
import { RendezVousService, RendezVous } from '../services/rendezvous.service';
import { AuthentificationService } from '../services/authentification.service';
import { UtilisateurService, Utilisateur, Role } from '../services/utilisateur.service';
import { FilterPipe } from '../filter.pipe';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { NotificationService, Notification } from '../services/notification.service';

enum EtatRdv {
  EnAttente = 'en attente',
  Confirme = 'confirmé',
  Annule = 'annulé'
}

@Component({
  selector: 'app-gestion-rendez-vous',
  templateUrl: './gestion-rendez-vous.component.html',
  styleUrls: ['./gestion-rendez-vous.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FullCalendarModule, FilterPipe, FormsModule],
  animations: [
    trigger('fadeSlide', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ])
  ]
})
export class GestionRendezVousComponent implements OnInit {
  formVisible: boolean = false;
  searchText: string = '';
  rdvForm: FormGroup;
  rendezVousList: RendezVous[] = [];
  patients: Utilisateur[] = [];
  selectedEvent: RendezVous | null = null;
  logoPath = './assets/images/logo.png';
  medecinId: string | null = null;
  currentMedecin: Utilisateur | null = null; 


  toggleForm() {
    this.formVisible = !this.formVisible;
  }

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
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    eventClick: this.handleEventClick.bind(this)
  };

  constructor(
    private fb: FormBuilder,
    private rdvService: RendezVousService,
    public authService: AuthentificationService,
    private router: Router,
    private utilisateurService: UtilisateurService,
    private notificationService: NotificationService
  ) {
    this.rdvForm = this.fb.group({
      date: ['', Validators.required],
      titre: ['', [Validators.required, Validators.minLength(3)]],
      etat: [EtatRdv.EnAttente, Validators.required],
      descriptionRdv: [''],
      patientId: [null, Validators.required],
      medecinId: [null]
    });
  }

  ngOnInit(): void {
    this.medecinId = sessionStorage.getItem('userId');
    this.fetchCurrentMedecin();
    if (this.medecinId) {
      this.rdvForm.patchValue({ medecinId: this.medecinId });
      this.fetchPatients();
      this.fetchRendezVousList();
    }
  }

  fetchCurrentMedecin(): void {
    if (!this.medecinId) return;

    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs: Utilisateur[]) => {
        const id = Number(this.medecinId);
        this.currentMedecin = utilisateurs.find(u => u.id === id) || null;
        if (this.currentMedecin) {
          console.log('Patient connecté :', this.currentMedecin);
        } else {
          console.warn('Utilisateur non trouvé pour ID :', id);
        }
      },
      error: (err) => {
        console.error('Erreur lors de la récupération des utilisateurs :', err);
      }
    });
  }


  getNomMedecin(): string {
    return this.currentMedecin ? `${this.currentMedecin.firstName} ${this.currentMedecin.lastName}` : 'Médecin';
  }

  onSubmit(): void {
    if (this.rdvForm.valid) {
      const formData = this.rdvForm.value;

      this.rdvService.create(formData).subscribe({
        next: (response: RendezVous) => {
          this.rendezVousList.push(response);
          this.updateCalendarEvents();

          const nomMedecin = this.getNomMedecin();
          const dateRdv = new Date(response.date).toLocaleString('fr-FR');

          const notification: Notification = {
            id: 0,
            titre: 'Nouveau rendez-vous',
            contenu: `Vous avez un nouveau rendez-vous le ${dateRdv} chez le médecin ${nomMedecin}. Merci de confirmer votre rendez-vous.`,
            dateNotification: new Date().toISOString(),
            reciver: response.patientId,
            sender: response.medecinId
          };

          this.notificationService.createNotification(notification).subscribe({
            next: () => alert('Notification envoyée.'),
            error: (err) => console.error('Erreur lors de l’envoi de la notification :', err)
          });

          this.rdvForm.reset({
            etat: EtatRdv.EnAttente,
            medecinId: this.medecinId
          });

          alert('Rendez-vous ajouté avec succès');
        },
        error: (error: any) => {
          console.error('Erreur lors de l\'ajout du rendez-vous :', error);
        }
      });
    }
  }

  fetchPatients(): void {
    const idMedecin = this.medecinId ? Number(this.medecinId) : null;
    this.utilisateurService.getAllUsers().subscribe({
      next: (utilisateurs) => {
        this.patients = utilisateurs.filter(p => p.ajoutePar === idMedecin);
      },
      error: (err) => {
        console.error('Erreur lors du chargement des utilisateurs :', err);
      }
    });
  }

  getNomPatient(patientId: number | null): string {
    const patient = this.patients.find(p => p.id === patientId);
    return patient ? `${patient.firstName} ${patient.lastName}` : 'Inconnu';
  }

  fetchRendezVousList(): void {
    const currentUserId = this.medecinId;
    if (!currentUserId) return;

    this.rdvService.getAll().subscribe({
      next: (data: RendezVous[]) => {
        this.rendezVousList = data.filter(rdv => Number(rdv.medecinId) === Number(currentUserId));
        this.updateCalendarEvents();
      },
      error: (error: any) => {
        console.error('Erreur lors de la récupération des rendez-vous :', error);
      }
    });
  }

  fetchAllRendezVous(): void {
    this.rdvService.getAll().subscribe({
      next: (data: RendezVous[]) => {
        this.rendezVousList = data;
        this.updateCalendarEvents();
      },
      error: (error: any) => {
        console.error('Erreur lors de la récupération des rendez-vous :', error);
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

  confirmerRendezVous(rdv: RendezVous): void {
    const updated = { ...rdv, etat: EtatRdv.Confirme };
    this.rdvService.update(updated).subscribe({
      next: () => {
        rdv.etat = EtatRdv.Confirme;
        this.updateCalendarEvents();
      },
      error: (error: any) => {
        console.error('Erreur lors de la confirmation :', error);
      }
    });
  }

  annulerRendezVous(rdv: RendezVous): void {
    const updated = { ...rdv, etat: EtatRdv.Annule };
    this.rdvService.update(updated).subscribe({
      next: () => {
        rdv.etat = EtatRdv.Annule;
        this.updateCalendarEvents();
      },
      error: (error: any) => {
        console.error('Erreur lors de l\'annulation :', error);
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

  logout(event: Event): void {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
