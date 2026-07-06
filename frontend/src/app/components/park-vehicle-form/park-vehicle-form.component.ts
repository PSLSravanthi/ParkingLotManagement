import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { VehicleType } from '../../models/parking.models';

@Component({
  selector: 'app-park-vehicle-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './park-vehicle-form.component.html',
  styleUrl: './park-vehicle-form.component.css'
})
export class ParkVehicleFormComponent {
  @Output() park = new EventEmitter<{ vehicleNumber: string; vehicleType: VehicleType }>();

  readonly vehicleTypes: VehicleType[] = ['Motorcycle', 'Car', 'Bus'];

  readonly form;

  constructor(private readonly fb: FormBuilder) {
    this.form = this.fb.nonNullable.group({
      vehicleNumber: ['', [Validators.required, Validators.minLength(2)]],
      vehicleType: ['Car' as VehicleType, Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { vehicleNumber, vehicleType } = this.form.getRawValue();
    this.park.emit({ vehicleNumber, vehicleType });
    this.form.reset({ vehicleNumber: '', vehicleType: 'Car' });
  }
}
