import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {}; // conterrà lo username e la password da passare a un metodo che li invierà al server

  constructor(private authService: AuthService) {}

  ngOnInit() {}

  // viene richiamato il servizio authService contenente il metodo login
  // next: login corretto
  // error: errore nel login
  login() {
    this.authService.login(this.model).subscribe(
      (next) => {
        console.log('Logged in successfully');
      },
      (error) => {
        console.log('Failed to login');
      }
    );
  }

  // quanto lo user é loggato
  loggedIn() {
    const token = localStorage.getItem('token'); // ottiene il valore dell'attuale token con cui lo user é loggato
    return !!token; // true se token é presente, se é vuoto ritorna falso
  }

  // quando lo user esegue il logout
  logout() {
    localStorage.removeItem('token'); // il token viene rimosso
    console.log('Logged out');
  }
}
