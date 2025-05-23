import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableauPatientComponent } from './tableau-patient.component';

describe('TableauPatientComponent', () => {
  let component: TableauPatientComponent;
  let fixture: ComponentFixture<TableauPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableauPatientComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableauPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
