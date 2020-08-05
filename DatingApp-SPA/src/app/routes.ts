// file che viene usato per le routes

import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';

// ad ogni route forniamo un path a cui é collegato un componente
// quando uno user inserisce l'url o clicca in un link, aggiunge qualcosa al percorso (es: /members per i membri)
// se nessuno dei precendenti percorsi é corretto, userà la wildcard (**)
// Attenzione! La wildcard deve sempre essere messa per ultima!

// se modifichiamo la route manualmente e non siamo loggati, il metodo canActivate riporta alla home
export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '', // assumerà il valore di una delle child route (es: localhost:4200/members)
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      // array di routes
      {
        path: 'members',
        component: MemberListComponent,
        resolve: { users: MemberListResolver },
      },
      // specifichiamo il parametro della route (id)
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        resolve: { user: MemberDetailResolver },
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        resolve: {
          user: MemberEditResolver,
        },
        canDeactivate: [PreventUnsavedChanges],
      },
      { path: 'messages', component: MessagesComponent },
      { path: 'lists', component: ListsComponent },
    ],
  },

  { path: '**', redirectTo: '', pathMatch: 'full' },
  // in tutti gli altri casi, redireziona al localhost:4200 (che é poi la home)
];
