import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ValuesComponent } from './values/values.component';

/*NgModule bootstrappa il componente Angular
Quando creiamo un component, viene aggiunto automaticamente qui e viene importato*/
@NgModule({
  declarations: [AppComponent, ValuesComponent],
  imports: [BrowserModule, HttpClientModule] /*importante l'httpclientmodule*/,
  providers: [],
  bootstrap: [
    AppComponent,
  ] /*classe di angular che fornisce i dati per la view app.Component.html*/,
})
export class AppModule {}
