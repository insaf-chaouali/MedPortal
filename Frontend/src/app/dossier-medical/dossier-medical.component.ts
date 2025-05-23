import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DossierMedicalService } from '../services/dossier-medical.service';
import { UtilisateurService , Utilisateur } from '../services/utilisateur.service';
import { FilterPipe } from '../filter.pipe';
import { FullCalendarModule } from '@fullcalendar/angular';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-dossier-medical',
  standalone: true,
  templateUrl: './dossier-medical.component.html',
  styleUrls: ['./dossier-medical.component.css'],
  imports: [CommonModule, ReactiveFormsModule, FullCalendarModule, FilterPipe, FormsModule],
})
export class DossierMedicalComponent implements OnInit {
  dossierForm!: FormGroup;
  groupesSanguins: string[] = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];
  patients: any[] = [];
  medecinId!: string | null;
  mode: 'creer' | 'modifier' = 'creer';
  searchText: string = '';
  pdfListWithPatient: { fileName: string; downloadUrl: string; patientName: string }[] = [];
  currentMedecin: Utilisateur | null = null; 


  constructor(
    private fb: FormBuilder,
    private dossierService: DossierMedicalService,
    private utilisateurService: UtilisateurService,
    private router: Router

  ) {}

  ngOnInit(): void {
    this.initForm();
    this.loadMedecinId();
    this.fetchPatients().then(() => {
      this.loadAllPdfs(); 
      this.fetchCurrentMedecin();
    });
  }

  initForm(): void {
    this.dossierForm = this.fb.group({
      taille: [''],
      poids: [''],
      groupeSanguin: [''],
      antecedents: [''],
      traitements: [''],
      allergies: [''],
      observations: [''],
      medecinId: [''],
      patientId: ['', Validators.required]
    });
  }

  loadMedecinId(): void {
    this.medecinId = sessionStorage.getItem('userId');
    if (this.medecinId) {
      this.dossierForm.patchValue({ medecinId: this.medecinId });
    }
  }
  fetchCurrentMedecin(): void {
    this.medecinId = sessionStorage.getItem('userId');
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

  fetchPatients(): Promise<void> {
    return new Promise((resolve, reject) => {
      const idMedecinString = sessionStorage.getItem('userId');
      const idMedecin = idMedecinString ? Number(idMedecinString) : null;

      this.utilisateurService.getAllUsers().subscribe({
        next: (utilisateurs) => {
          this.patients = utilisateurs.filter(p => p.ajoutePar === idMedecin);
          resolve();
        },
        error: (err) => {
          console.error('Erreur lors du chargement des utilisateurs :', err);
          reject(err);
        }
      });
    });
  }

  downloadPdf(url: string): void {
    const link = document.createElement('a');
    link.href = url;
    link.download = '';
    link.click();
  }

  loadAllPdfs(): void {
    this.dossierService.getAllPdfs().subscribe({
      next: (pdfs) => {
        const pdfsToLoad = pdfs.map((pdf: any) => {
          const match = pdf.fileName.match(/dossier_(\d+)/);
          const dossierId = match ? Number(match[1]) : null;

          if (!dossierId) return null;

          return this.dossierService.getDossierById(dossierId).toPromise().then((dossier) => {
            const patient = this.patients.find(p => p.id === dossier.patientId);
            const patientName = patient ? `${patient.firstName} ${patient.lastName}` : 'Inconnu';
            return {
              fileName: pdf.fileName,
              downloadUrl: pdf.downloadUrl,
              patientName: patientName
            };
          });
        }).filter(Boolean);

        Promise.all(pdfsToLoad).then(results => {
          this.pdfListWithPatient = results as any[];
        });
      },
      error: (err) => {
        console.error('Erreur lors du chargement des PDFs:', err);
      }
    });
  }

  onSubmit(): void {
    if (this.dossierForm.invalid) return;

    const dossierData = this.dossierForm.value;

    this.dossierService.createDossier(dossierData).subscribe({
      next: res => {
        alert('Dossier médical créé avec succès.');
        this.dossierForm.reset();
        this.dossierForm.patchValue({ medecinId: this.medecinId });
      },
      error: err => {
        alert('Erreur lors de la création du dossier.');
        console.error(err);
      }
    });
  }
  logout(event: Event): void {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
