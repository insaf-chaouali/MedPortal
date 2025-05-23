import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageService, Message } from '../services/message.service';
import { UtilisateurService, Utilisateur } from '../services/utilisateur.service';

@Component({
  selector: 'app-message-patient',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './message-patient.component.html',
  styleUrls: ['./message-patient.component.css']
})
export class MessagePatientComponent implements OnInit {
  messages: Message[] = [];
  newMessage: string = '';
  envoyeurId: number = 0; // ID médecin
  receveurId: number = 0; // ID patient
  patientNom: string = 'Medecin';

  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute,
    private utilisateurService: UtilisateurService
  ) {}

 ngOnInit(): void {
  const idPatientString = sessionStorage.getItem('userId');
  this.envoyeurId = idPatientString ? Number(idPatientString) : 0; // patient connecté

  // On récupère le patient connecté pour connaître son médecin
  if (this.envoyeurId) {
    this.utilisateurService.getUserById(this.envoyeurId).subscribe(patient => {
      if (!patient) {
        console.error('Patient non trouvé');
        return;
      }
      
      if (!patient.ajoutePar) {
        console.error('Médecin non défini pour ce patient');
        return;
      }

      this.receveurId = patient.ajoutePar; // médecin qui a ajouté le patient

      this.utilisateurService.getUserById(this.receveurId).subscribe(medecin => {
        if (!medecin) {
          console.error('Médecin non trouvé');
          return;
        }

        this.patientNom = medecin.firstName + ' ' + medecin.lastName;

        this.loadConversation();
      });
    });
  }
}




  loadConversation(): void {
    this.messageService.getConversation(this.envoyeurId, this.receveurId).subscribe(data => {
      this.messages = data;
    });
  }
goBack(): void {
  window.history.back();
}

sendMessage(): void {
  if (this.newMessage.trim()) {
    const message: Omit<Message, 'id' | 'dateEnvoi'> = {
      envoyeurId: this.envoyeurId,
      receveurId: this.receveurId,
      contenu: this.newMessage
    };

    this.messageService.sendMessage(message).subscribe({
      next: (sent) => {
        this.messages.push(sent);
        this.newMessage = '';
      },
      error: (err) => {
        console.error('Erreur lors de l\'envoi du message :', err);
        window.alert('Erreur lors de l\'envoi du message.');
      }
    });
  }
}



  isSentBySelf(msg: Message): boolean {
    return msg.envoyeurId === this.envoyeurId;
  }
}
