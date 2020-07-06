import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root',
})
export class AlertifyService {
  constructor() {}

  // metodo di conferma, mesaggio di conferma
  confirm(message: string, okCallback: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        // evento: se lo user clicca il tasto ok, esce il messaggio
        okCallback();
      } else {
      }
    });
  }

  // vedi nav.component.ts, register.component.ts ecc

  // alert di successo (login di successo)
  success(message: string) {
    alertify.success(message);
  }

  // alert di errore (errore di login)
  error(message: string) {
    alertify.error(message);
  }

  // alert di avviso
  warning(message: string) {
    alertify.warning(message);
  }

  // messaggio semplice (logout)
  message(message: string) {
    alertify.message(message);
  }
}
