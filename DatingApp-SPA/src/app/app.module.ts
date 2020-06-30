import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

// tutti gli import legati all'applicazione per i componenti angular vengono registrati qui

/*NgModule bootstrappa il componente Angular
Quando creiamo un component, viene aggiunto automaticamente qui e viene importato*/
@NgModule({
  declarations: [AppComponent, NavComponent, HomeComponent, RegisterComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule],
  providers: [AuthService],
  bootstrap: [
    AppComponent,
    /*classe di angular che fornisce i dati per la view app.Component.html*/
  ],
})
export class AppModule {}
