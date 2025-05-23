import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AdminService, Utilisateur, Role } from '../services/admin-service';

@Component({
  selector: 'app-register-medecin',
  templateUrl: './register-medecin.component.html',
  styleUrls: ['./register-medecin.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class RegisterMedecinComponent implements OnInit {
  signupForm!: FormGroup;
  errorMessage: string = '';
  isLoading: boolean = false;
  userId: number = 0;

  constructor(
    private fb: FormBuilder, 
    private router: Router, 
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    // Récupération de l'ID utilisateur depuis la session
    const storedId = sessionStorage.getItem("userId");
    this.userId = storedId ? parseInt(storedId, 10) : 0;
    
    this.signupForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(3)]],
      specialite: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      lastName: ['', Validators.required],
      firstName: ['', Validators.required],
      birthDate: ['', Validators.required],
      gender: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern(/^\d{8}$/)]],
      address: [''],
      city: ['', Validators.required],
      postalCode: ['', [Validators.required, Validators.pattern(/^\d{4}$/)]],
      termsAccepted: [false, Validators.requiredTrue],
      ajoutePar: [this.userId]
    }, { validators: this.passwordMatchValidator });
  }
  
  passwordMatchValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    return password && confirmPassword && password !== confirmPassword 
      ? { passwordMismatch: true } 
      : null;
  }

  onSubmit(): void {
    if (this.signupForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';

      const formValue = this.signupForm.value;

      // Mise à jour d'AjoutePar en cas de changement
      const utilisateurData: Utilisateur = {
        login: formValue.login,
        specialite: formValue.specialite,
        email: formValue.email,
        password: formValue.password,
        lastName: formValue.lastName,
        firstName: formValue.firstName,
        birthDate: formValue.birthDate,
        gender: formValue.gender,
        phone: formValue.phone,
        address: formValue.address,
        city: formValue.city,
        postalCode: formValue.postalCode,
        role: Role.Médecin,
        ajoutePar: this.userId // récupéré proprement depuis ngOnInit
      };

      console.log('Attempting to register Medecin...', utilisateurData);

      this.adminService.registerMédecin(utilisateurData).subscribe({
        next: (response) => {
          console.log('Registration successful', response);
          window.location.reload();
        },
        error: (error) => {
          console.error('Registration failed', error);
          this.errorMessage = error.message || 'Registration failed. Please try again.';
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    } else {
      this.markFormGroupTouched(this.signupForm);
    }
  }

  private markFormGroupTouched(formGroup: FormGroup) {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  logout(event: Event) {
    event.preventDefault();
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
