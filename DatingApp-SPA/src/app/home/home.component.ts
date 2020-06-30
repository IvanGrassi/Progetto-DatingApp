import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  registerMode = false;
  // values: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    // this.getValues();
  }

  registerToggle() {
    this.registerMode = true; // verrà usato come switch per vedere i campi del form
  }

  // sostituito dall'auth.service.ts

  // recupera i valori a questo indirizzo (dall'api)
  // getValues() {
  //  this.http.get('http://localhost:5000/api/values').subscribe(
  //    (response) => {
  //      this.values = response; /*contiene gli oggetti che stiamo girando alla nostra api e li passiamo tramite risposta alla chiamata*/
  //    },
  //    (error) => {
  //      console.log(error); /*in caso di errore viene stampato un log*/
  //    }
  //  );
  // }

  // conterrà un false
  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }
}
