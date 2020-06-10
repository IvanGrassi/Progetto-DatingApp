import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css'],
})
export class ValuesComponent implements OnInit {
  values: any; /*any permette di immettere nella variabile qualsiasi valore*/

  /*HttpClient va aggiunto qui*/
  constructor(private http: HttpClient) {}

  /** quando viene chiamata l'api */
  ngOnInit() {
    this.getValues();
  }

  /*recupera i valori a questo indirizzo */
  getValues() {
    this.http.get('http://localhost:5000/api/values').subscribe(
      (response) => {
        this.values = response; /*contiene gli oggetti che stiamo girando alla nostra api*/
      },
      (error) => {
        console.log(error); /*in caso di errore viene stampato un log*/
      }
    );
  }
}
