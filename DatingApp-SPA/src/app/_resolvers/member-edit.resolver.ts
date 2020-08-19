import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    // quando si usa resolve: usiamo il metodo getUser per ottnere lo user che corrisponde a quell'id
    return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
      catchError((error) => {
        // in caso di errore:
        this.alertify.error('Problem retrieving your data');
        this.router.navigate(['/members']); // user viene redirezionato alla pagina members
        return of(null); // e non viene ritornato alcun dato
      })
    );
  }
}