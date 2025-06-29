import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt'
registerLocaleData(localePt)

import { ListaComponent } from '../produtos/lista/lista.component';
import { ProdutoComponent } from '../produtos/produto/produto.component';

import { ProdutoService } from './services/produtos.service';


@NgModule({
  declarations: [
    ListaComponent,
    ProdutoComponent,
  ],
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule
  ],
  providers: [
    ProdutoService
  ]
})
export class ProdutosModule { }