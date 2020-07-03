import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

// file che permette di catturare l'errore nell'interceptor manipolando la risposta
// che vogliamo far visualizzare sul browser

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  // intercettiamo le richieste (req) e catturiamo gli errori che avverranno (next)
  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        // viene catturato l'errore
        if (error.status === 401) {
          // se l'errore é un 401
          return throwError(error.statusText); // mostriamo il testo dell'errore
        }
        if (error instanceof HttpErrorResponse) {
          // SE l'oggetto é un isstanza di HTTPErrorResponse..
          const applicationError = error.headers.get('Application-Error'); // lo recuperiamo dall'header Application-Error
          if (applicationError) {
            // se l'app-error non é vuoto (TUTTI gli errori 500)
            return throwError(applicationError); // viene restituito e lanciato l'errore
          }

          // errori di modelState
          const serverError = error.error; // se guardo in console: HttpErrorResponse.error quindi error.error
          let modalStateErrors = ''; // memorizza gli errori del model state

          // HttpErrorResponse.error.errors, abbiamo bisogno degli oggetti contenuti in errors
          if (serverError.errors && typeof serverError.errors === 'object') {
            for (const key in serverError.errors) {
              // per ogni chiave di errore presente ..
              if (serverError.errors[key]) {
                // ottengo la chiave dell'errore
                // se la pw é troppo corta ad esempio: la key é Password (.../error/errors/Password) a cui é collegato un valore
                modalStateErrors += serverError.errors[key] + '\n';
                // viene costruita una lista di stringhe separata da newline per ogni key
                // per ogni errore di ModelState
              }
            }
          }
          // ritorniamo se modalstateErrors é != da '' oppure se
          // é presente un serverError
          // se entrambi non sono presenti: scritta Server Error
          return throwError(modalStateErrors || serverError || 'Server Error');
        }
      })
    );
  }
}

// esportiamo e registriamo un nuovo InterceptorProvider all'angular HTTPInterceptor array di provider
export const ErrorInterceptorProvider = {
  provide: HTTP_INTERCEPTORS, // forniamo la key
  useClass: ErrorInterceptor, // ErrorInterceptorClass
  multi: true, // l'HttpInterceptor può avere multipli interceptor
};
