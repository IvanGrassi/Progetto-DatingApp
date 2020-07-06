import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root', // app.module.ts é root
})
export class AuthService {
  // injectable permette di iniettare ... nel nostro servizio

  baseUrl = 'http://localhost:5000/api/auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any; // usata per fare in modo di visualizzare il nome nella navbar (Welcome ...)

  // dichiarato l'uso dell'HttpClient
  constructor(private http: HttpClient) {}

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
            // tutte le volte a seguire in quanto fa parte del local storage del sito web
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            console.log(this.decodedToken);
          }
        })
      );
  }

  register(model: any) {
    // ritorna la richiesta post contenente i dati
    return this.http.post(this.baseUrl + 'register', model);
  }

  // verifica se lo user é loggato
  loggedIn() {
    const token = localStorage.getItem('token'); // recupera il valore del token dal localstorage
    return !this.jwtHelper.isTokenExpired(token); // ritorna un true/false in base al token (se scaduto = false, altrimenti true)
  }
}
