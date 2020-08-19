import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {}; // conterrà lo username e la password da passare a un metodo che li invierà al server
  photoUrl: string;

  constructor(
    public authService: AuthService, // importante che sia public
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(
      (photoUrl) => (this.photoUrl = photoUrl)
    );
  }

  // viene richiamato il servizio authService contenente il metodo login
  // next: i dati vengono inoltrati correttamente
  // error: errore nel login
  login() {
    this.authService.login(this.model).subscribe(
      (next) => {
        this.alertify.success('Logged in successfully');
      },
      (error) => {
        this.alertify.error(error);
      },
      () => {
        this.router.navigate(['/members']); // la route cambia in /members
      }
    );
  }

  // quanto lo user é loggato
  loggedIn() {
    // const token = localStorage.getItem('token'); // ottiene il valore dell'attuale token con cui lo user é loggato
    // return !!token; // true se token é presente, se é vuoto ritorna falso
    return this.authService.loggedIn(); // auth.service.ts
  }

  // quando lo user esegue il logout
  logout() {
    localStorage.removeItem('token'); // il token viene rimosso
    localStorage.removeItem('user'); // i dati dello user vengono rimossi
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('Logged out');
    this.router.navigate(['/home']); // torna alla homepage (route /home)
  }
}
