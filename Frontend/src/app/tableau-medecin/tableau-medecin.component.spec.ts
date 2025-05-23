import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableauMedecinComponent } from './tableau-medecin.component';

describe('TableauPatientComponent', () => {
  let component: TableauMedecinComponent;
  let fixture: ComponentFixture<TableauMedecinComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableauMedecinComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableauMedecinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
