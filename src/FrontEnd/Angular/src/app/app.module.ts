import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ListaComponent } from './produtos/lista/lista.component';
import { ProdutoComponent } from './produtos/produto/produto.component';

import { NavegacaoModule } from './navegacao/navegacao.module';

import { NotificacaoService } from './services/notificacao.service';
import { NotificacaoComponent } from './components/notificacao/notificacao.component';

@NgModule({
  declarations: [
    AppComponent,
    ListaComponent,
    ProdutoComponent,
    NotificacaoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NavegacaoModule,
  ],
  providers: [
    NotificacaoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
