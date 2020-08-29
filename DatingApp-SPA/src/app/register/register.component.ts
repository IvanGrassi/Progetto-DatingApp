import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // viene creata una nuova istanza della classe
  user: User;
  registerForm: FormGroup; // formGroup definisce i form in angular
  bsConfig: Partial<BsDatepickerConfig>; // permette di modificare lo stile del datepicker,
  // partial class perché cosi tutte le prop del datepicker sono opzionali

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'DD/MM/YYYY',
    };
    this.createRegisterForm();
  }

  createRegisterForm() {
    // definisco i campi del form di registrazione (bottone Register) + Validazione
    // più validazioni vengono fatte tramite array
    this.registerForm = this.fb.group(
      {
        gender: ['male'], // radio button
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(8),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      { validator: this.passwordMatchValidator }
    );
  }

  // permetterà di validare la pw se il contenuto dei form password- conferma password é UGUALE
  passwordMatchValidator(g: FormGroup) {
    // se sono uguali: restituiamo null. Altrimenti ritorna un oggetto con key (mismatch) e con valore (true)
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    // se il form é stato compilato correttamente con tutti i campi required
    if (this.registerForm.valid) {
      // passiamo i valori dei form allo user object, assegniamo l'oggetto vuoto {} allo user
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => {
          this.alertify.success('Registration successful');
        },
        (error) => {
          this.alertify.error(error);
        },
        () => {
          // dopo aver effettuato la registrazione correttamente: prima viene eseguito il login con le credenziali dello user creato
          // poi viene reindirizzato alla pagina members
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          });
        }
      );
    }
  }

  // non inserisco user e clicco cancel
  cancel() {
    this.cancelRegister.emit(false); // emette evento settando cancelRegister = false (home.component.html);
  }
}
