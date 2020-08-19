import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';

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
    const token = localStorage.getItem('token'); // recupero e memorizzazione token dal locastorage
    const user: User = JSON.parse(localStorage.getItem('user')); // come sopra ma con lo user, parse per trasformare l'oggetto in stringa
    if (token) {
      // e se il token esiste: viene impostato come decoded token
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if (user) {
      this.authService.currentUser = user;
      // aggiorna la currentPhoto in authService con la currentUserPhoto contenuta in localstorage
      this.authService.changeMemberPhoto(user.photoUrl);
    }
  }
}
