import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  // costruttore per iniettare l'authService
  constructor(private authService: AuthService) {}

  // letto non appena il componente viene caricato all'avvio dell'esecuzione dell'app
  ngOnInit() {
    const token = localStorage.getItem('token'); // recupero token dal locastorage
    if (token) {
      // e se il token esiste: viene impostato come decoded token
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
