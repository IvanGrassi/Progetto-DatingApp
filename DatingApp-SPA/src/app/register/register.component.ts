import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter(); // viene creata una nuova istanza della classe
  model: any = {};

  constructor(private authService: AuthService) {}

  ngOnInit(): void {}

  // registrazione user
  register() {
    this.authService.register(this.model).subscribe(
      () => {
        console.log('Registration successful');
      },
      (error) => {
        console.log(error);
      }
    );
  }

  // non inserisco user e clicco cancel
  cancel() {
    this.cancelRegister.emit(false); // emette evento settando cancelRegister = false (home.component.html);
    console.log('Cancelled');
  }
}
