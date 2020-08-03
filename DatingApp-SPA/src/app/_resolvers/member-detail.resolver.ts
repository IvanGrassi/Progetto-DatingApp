import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  // un resolver é un interfaccia implementabile dalle classi per essere un data provider
  // risolve la route in un percorso che definiamo in routes.ts
  // observable é ciò che ritorniamo

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    // quando si usa resolve: usiamo il metodo getUser per ottnere lo user che corrisponde a quell'id
    return this.userService.getUser(route.params.id).pipe(
      catchError((error) => {
        // in caso di errore:
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/members']); // user viene redirezionato alla pagina members
        return of(null); // e non viene ritornato alcun dato
      })
    );
  }
}
