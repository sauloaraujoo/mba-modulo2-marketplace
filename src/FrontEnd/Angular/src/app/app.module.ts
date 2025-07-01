import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { NavegacaoModule } from './navegacao/navegacao.module';

import { NotificacaoService } from './services/notificacao.service';
import { NotificacaoComponent } from './components/notificacao/notificacao.component';

@NgModule({
  declarations: [
    AppComponent,
    NotificacaoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NavegacaoModule,
    HttpClientModule
  ],
  providers: [
    NotificacaoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
