import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  // permette di accedere al form
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  @HostListener('window:beforeunload', ['$event'])
  // il listener ascolta l'host e intraprende un azione (avvisa che si sta uscendo dalla pagina
  // se clicco su un altra sezione mentre i form sono stati modificati)
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      // tslint:disable-next-line: deprecation
      event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UserService, // permette l'uso del metodo update user presente in UserService.ts
    private authService: AuthService
  ) {}

  ngOnInit() {
    // recuperiamo i dati dalla route (prima bisogna sottoscriverla)
    this.route.data.subscribe((data) => {
      this.user = data.user;
    });
  }

  // metodo che permette di aggiornare i dati dello user,
  // verrà eseguito sul percoros definito in user.service.ts
  updateUser() {
    // nameid = id dello user, ritorna un observable
    this.userService
      .updateUser(this.authService.decodedToken.nameid, this.user)
      .subscribe(
        (next) => {
          this.alertify.success('Profile updated successfully!');
          // mi permette di resettare il form (la label di alert in caso di modifica viene disattivata di nuovo)
          // e con this.user, rimane visualizzato il testo che avevo modificato
          this.editForm.reset(this.user);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
}
