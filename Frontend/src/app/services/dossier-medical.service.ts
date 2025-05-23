import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DossierMedicalService {
  private apiUrl = 'http://localhost:5127/api/DossierMedical';

  constructor(private http: HttpClient) {}

  // Récupérer tous les dossiers médicaux
  getAllDossiers(): Observable<any> {
    return this.http.get(`${this.apiUrl}`);
  }

  // Récupérer un dossier par l'ID du dossier
  getDossierById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  // Récupérer un dossier par l'ID du patient
  getDossierByPatientId(patientId: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/patient/${patientId}`);
  }

  // Créer un dossier (si séparé du saveDossier)
  createDossier(dossier: any): Observable<any> {
    return this.http.post(`${this.apiUrl}`, dossier);
  }

  // Mettre à jour un dossier existant
  updateDossier(id: number, dossier: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dossier);
  }

  // Créer ou mettre à jour un dossier pour un patient
  saveDossier(patientId: string, dossier: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/patient/${patientId}`, dossier);
  }

  // Supprimer un dossier par ID
  deleteDossier(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  // Télécharger un PDF
  downloadPdf(id: number): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/pdf/${id}`, {
      responseType: 'blob',
    });
  }

  // Obtenir tous les PDFs
  getAllPdfs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/pdf`);
  }
}
