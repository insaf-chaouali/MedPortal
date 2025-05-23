import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DossierMedicalPatientComponent } from './dossier-medical-patient.component';

describe('DossierMedicalComponent', () => {
  let component: DossierMedicalPatientComponent;
  let fixture: ComponentFixture<DossierMedicalPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DossierMedicalPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DossierMedicalPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
