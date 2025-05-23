import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GestionRendezVousComponent } from './gestion-rendez-vous.component';

describe('GestionRendezVousComponent', () => {
  let component: GestionRendezVousComponent;
  let fixture: ComponentFixture<GestionRendezVousComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GestionRendezVousComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GestionRendezVousComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
