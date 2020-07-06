import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // viene creata una nuova istanza della classe
  model: any = {};

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {}

  // registrazione user
  register() {
    this.authService.register(this.model).subscribe(
      () => {
        this.alertify.success('Registration successful');
      },
      (error) => {
        this.alertify.error(error); // username/password troppo corta, mancante, ecc..
      }
    );
  }

  // non inserisco user e clicco cancel
  cancel() {
    this.cancelRegister.emit(false); // emette evento settando cancelRegister = false (home.component.html);
  }
}
