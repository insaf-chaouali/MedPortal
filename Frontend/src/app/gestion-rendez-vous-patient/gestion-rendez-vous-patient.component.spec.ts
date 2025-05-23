import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionRendezVousPatientComponent } from './gestion-rendez-vous-patient.component';

describe('GestionRendezVousComponent', () => {
  let component: GestionRendezVousPatientComponent;
  let fixture: ComponentFixture<GestionRendezVousPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionRendezVousPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionRendezVousPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
