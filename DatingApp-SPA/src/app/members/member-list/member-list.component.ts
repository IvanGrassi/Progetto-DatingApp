import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user')); // recupera i dati dello user contenuti nel localstorage
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];
  userParams: any = {}; // empty object
  pagination: Pagination;

  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  // recuperiamo i dati dalla route (prima bisogna sottoscriverla)
  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.users = data.users.result;
      this.pagination = data.users.pagination; // accesso alle proprietà dello user (pagination.ts)
    });
    // se loggo come male, allora vedrò le female
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.userParams.orderBy = 'lastActive'; // ordinamento di default
  }

  // azzera i filtri all visualizzazione di default ricaricando gli users
  resetFilters() {
    this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female';
    this.userParams.minAge = 18;
    this.userParams.maxAge = 99;
    this.loadUsers();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    // console.log('Page ' + this.pagination.currentPage);
    this.loadUsers();
  }

  // il metodo loadUsers non é più necessario perché i dati vengono ricevuti
  // dal MemberListComponent (fa già la route)

  loadUsers() {
    // recupera l'array degli users tramite metodo getUsers in base ai
    // dati impostati per la paginazione
    this.userService
      // getUsers comprende ancehe pageNumber e pageSize già settati
      .getUsers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.userParams
      )
      .subscribe(
        // viene ritornato un elenco di users paginati in base a quanto é stato scelto
        (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },
        (error) => {
          // e in caso di errore, viene visualizzato un messaggio di errore alertify (vedi alertify.service.ts)
          this.alertify.error(error);
        }
      );
  }
}
