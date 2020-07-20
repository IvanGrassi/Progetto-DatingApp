import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  canActivate(): // verifica se lo user é loggato
  boolean {
    // se siamo loggati ritorniamo true se user loggato oppure false
    if (this.authService.loggedIn()) {
      return true;
    }
    // e se non lo é
    this.alertify.error('You shall not pass!!!');
    this.router.navigate(['/home']); // lo rispedisco alla home
  }
  // Route guard permette di evitare all'utente di raggiungere una pagina inserendo manualmente il percorso
}
