import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';


import { ProdutoService } from './produtos/services/produtos.service';
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
    HttpClientModule
  ],
  providers: [
    NotificacaoService,
    ProdutoService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
