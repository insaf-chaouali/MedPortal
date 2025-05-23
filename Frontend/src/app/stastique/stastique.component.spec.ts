import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StastiqueComponent } from './stastique.component';

describe('StastiqueComponent', () => {
  let component: StastiqueComponent;
  let fixture: ComponentFixture<StastiqueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StastiqueComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StastiqueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
