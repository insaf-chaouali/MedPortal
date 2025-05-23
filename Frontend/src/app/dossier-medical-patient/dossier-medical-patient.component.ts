import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { DossierMedicalService } from '../services/dossier-medical.service';
import { Router } from '@angular/router';
import { NgIf, NgFor } from '@angular/common';
import { UtilisateurService , Utilisateur } from '../services/utilisateur.service';



@Component({
  selector: 'app-dossier-medical-patient',
  templateUrl: './dossier-medical-patient.component.html',
  styleUrls: ['./dossier-medical-patient.component.css'],
  standalone: true, 
  imports: [NgIf, NgFor]
})
export class DossierMedicalPatientComponent implements OnInit {
  pdfList: { fileName: string; downloadUrl: string }[] = [];
  errorMessage: string | null = null;
  currentPatient: Utilisateur | null = null;

  constructor(
    private dossierService: DossierMedicalService,
    private sanitizer: DomSanitizer,
    private router: Router,
    private utilisateurService: UtilisateurService
  ) {}

 ngOnInit(): void {
  const patientId = sessionStorage.getItem('userId');
  console.log('Patient ID:', patientId);
  this.fetchCurrentPatient();
  if (!patientId) {
    this.errorMessage = 'Utilisateur non identifié.';
    return;
  }

  this.dossierService.getAllDossiers().subscribe({
    next: (dossiers: any[]) => {
      console.log('Dossiers reçus:', dossiers);

      const dossiersPatient = dossiers.filter(d => String(d.patientId) === patientId);
      console.log('Dossiers filtrés du patient:', dossiersPatient);

      if (dossiersPatient.length === 0) {
        this.errorMessage = 'Aucun dossier trouvé pour ce patient.';
        return;
      }

      this.dossierService.getAllPdfs().subscribe({
        next: (pdfs: any[]) => {
          console.log('PDFs reçus:', pdfs);

          this.pdfList = [];
          dossiersPatient.forEach(dossier => {
            const pdf = pdfs.find(p => p.fileName.includes(`dossier_${dossier.id}`));
            if (pdf && pdf.downloadUrl) {
              this.pdfList.push({ fileName: pdf.fileName, downloadUrl: pdf.downloadUrl });
            }
          });
          console.log('PDFs filtrés pour le patient:', this.pdfList);

          if (this.pdfList.length === 0) {
            this.errorMessage = 'Aucun PDF disponible pour ce dossier.';
          }
        },
        error: (err) => {
          alert('Erreur lors du chargement des PDFs :');
          this.errorMessage = 'Erreur lors de la récupération des fichiers PDF.';
        }
      });
    },
    error: (err) => {
      alert('Erreur lors de la récupération des dossiers :');
      this.errorMessage = 'Erreur lors de la récupération des dossiers.';
    }
  });
}

downloadPdf(url: string): void {
  const link = document.createElement('a');
  link.href = url;
  link.target = '_blank';
  link.download = '';
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link);
}
fetchCurrentPatient(): void {
  const patientId = sessionStorage.getItem('userId');
  if (!patientId) return;

  this.utilisateurService.getAllUsers().subscribe({
    next: (utilisateurs: Utilisateur[]) => {
      const id = Number(patientId);
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
