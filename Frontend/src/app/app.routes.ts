import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeMedecinComponent } from './home-medecin/home-medecin.component';
import { HomePatientComponent } from './home-patient/home-patient.component';
import { RegisterPatientComponent } from './register-patient/register-patient.component';
import { LoginComponent } from './login/login.component';
import { GestionRendezVousComponent } from './gestion-rendez-vous/gestion-rendez-vous.component'; 
import { TableauPatientComponent} from './tableau-patient/tableau-patient.component'; 
import { DossierMedicalPatientComponent} from './dossier-medical-patient/dossier-medical-patient.component'; 
import { GestionRendezVousPatientComponent } from './gestion-rendez-vous-patient/gestion-rendez-vous-patient.component';
import { RegisterMedecinComponent } from './register-medecin/register-medecin.component';
import { TableauMedecinComponent } from './tableau-medecin/tableau-medecin.component'; 
import { MessageComponent } from './message/message.component'; 
import { ConversationComponent } from './conversation/conversation.component'; 
import { MessagePatientComponent } from './message-patient/message-patient.component'; 
import { ConversationPatientComponent } from './conversation-patient/conversation-patient.component'; 
import { DossierMedicalComponent} from './dossier-medical/dossier-medical.component'; 
import { NotificationPatientComponent} from './notification-patient/notification-patient.component';
import { NotificationComponent} from './notification/notification.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { StastiqueComponent } from './stastique/stastique.component';
import { EditMedecinComponent } from './edit-medecin/edit-medecin.component';
import { EditPatientComponent } from './edit-patient/edit-patient.component';



export const routes: Routes = [ 
 { path: 'home-medecin', component: HomeMedecinComponent },
 { path: 'home-patient', component: HomePatientComponent },
  { path: 'register', component: RegisterPatientComponent },
  { path: 'login', component: LoginComponent },  
  { path: 'gestion-rendez-vous', component: GestionRendezVousComponent }, 
  { path: 'tableau-patient', component: TableauPatientComponent },
  { path: 'dossier-medical', component: DossierMedicalComponent },
  { path: 'gestion-rendez-vous-patient', component: GestionRendezVousPatientComponent },
  { path: 'register-medecin', component: RegisterMedecinComponent },
  { path: 'tableau-medecin', component: TableauMedecinComponent },
  { path: 'message', component: MessageComponent },
  { path: 'conversation', component: ConversationComponent },
  { path: 'message-patient', component: MessagePatientComponent },
  { path: 'conversation-patient', component: ConversationPatientComponent },
  { path: 'dossier-medical-patient', component: DossierMedicalPatientComponent },
  { path: 'notification-patient', component: NotificationPatientComponent },
  { path: 'welcome', component: WelcomeComponent },
  { path: 'stastique', component: StastiqueComponent },
  { path: 'edit-medecin/:id', component: EditMedecinComponent },
  { path: 'edit-patient/:id', component: EditPatientComponent },
  { path: 'notification', component: NotificationComponent },


  { path: '', redirectTo: '/welcome', pathMatch: 'full' } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }