import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MessageService, Message } from '../services/message.service';
import { UtilisateurService, Utilisateur } from '../services/utilisateur.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {
  messages: Message[] = [];
  newMessage: string = '';
  envoyeurId: number = 0; // ID médecin
  receveurId: number = 0; // ID patient
  patientNom: string = 'Patient';

  constructor(
    private messageService: MessageService,
    private route: ActivatedRoute,
    private utilisateurService: UtilisateurService
  ) {}

  ngOnInit(): void {
  const idMedecin = sessionStorage.getItem('userId');
  this.envoyeurId = idMedecin ? Number(idMedecin) : 0;

  const patientIdRaw = this.route.snapshot.queryParamMap.get('patientId'); // récupère patientId depuis l'URL
  this.receveurId = patientIdRaw ? Number(patientIdRaw) : 0;

  if (this.receveurId) {
    this.utilisateurService.getUserById(this.receveurId).subscribe(user => {
      this.patientNom = user.firstName + ' ' + user.lastName;  // affiche prénom + nom
    });
    this.loadConversation();
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
