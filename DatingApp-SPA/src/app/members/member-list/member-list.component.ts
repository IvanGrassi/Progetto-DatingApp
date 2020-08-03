import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  users: User[];

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  // recuperiamo i dati dalla route (prima bisogna sottoscriverla)
  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.users = data.users;
    });
  }

  // il metodo loadUsers non é più necessario perché i dati vengono ricevuti
  // dal MemberListComponent (fa già la route)

  /*loadUsers() {
    // recupera l'array degli users tramite metodo getUsers
    this.userService.getUsers().subscribe(
      (users: User[]) => {
        this.users = users;
      },
      (error) => {
        // e in caso di errore, viene visualizzato un messaggio di errore alertify (vedi alertify.service.ts)
        this.alertify.error(error);
      }
    );
  }*/
}
