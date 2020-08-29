import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root', // app.module.ts é root
})
export class AuthService {
  // injectable permette di iniettare ... nel nostro servizio

  baseUrl = environment.apiUrl + 'auth/'; // vedi environment.ts
  jwtHelper = new JwtHelperService();
  decodedToken: any; // usata per fare in modo di visualizzare il nome nella navbar (Welcome ...)
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png'); // immagine di default dello user
  currentPhotoUrl = this.photoUrl.asObservable();

  // dichiarato l'uso dell'HttpClient
  constructor(private http: HttpClient) {}

  // permette di aggiornare la foto vicino a welcome, con la foto modificata in edit photos
  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  // metodo di login che riceve tramite variabile model, i dati inseriti nel form della navbar
  login(model: any) {
    return this.http
      .post(this.baseUrl + 'login', model) // ritorna la richiesta post contenente i dati
      .pipe(
        // la pipe combina multiple funzioni in una sola (rxjs)
        map((response: any) => {
          const user = response; // contiene il token della richiesta
          if (user) {
            localStorage.setItem('token', user.token); // permette di mantenere in modo persistente il token, può essere riutilizzato
            localStorage.setItem('user', JSON.stringify(user.user)); // setta il valore di tutti i dati dello user, foto inclusa
            // tutte le volte a seguire in quanto fa parte del local storage del sito web
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            this.currentUser = user.user;
            this.changeMemberPhoto(this.currentUser.photoUrl); // permette di accedere all'url della currentPhoto
          }
        })
      );
  }

  register(user: User) {
    // ritorna la richiesta post contenente i dati
    return this.http.post(this.baseUrl + 'register', user);
  }

  // verifica se lo user é loggato
  loggedIn() {
    const token = localStorage.getItem('token'); // recupera il valore del token dal localstorage
    return !this.jwtHelper.isTokenExpired(token); // ritorna un true/false in base al token (se scaduto = false, altrimenti true)
  }
}
