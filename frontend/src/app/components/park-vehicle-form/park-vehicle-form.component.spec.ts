import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParkVehicleFormComponent } from './park-vehicle-form.component';

describe('ParkVehicleFormComponent', () => {
  let component: ParkVehicleFormComponent;
  let fixture: ComponentFixture<ParkVehicleFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParkVehicleFormComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ParkVehicleFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
