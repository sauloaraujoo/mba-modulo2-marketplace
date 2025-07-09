import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt'
registerLocaleData(localePt)

import { ListaComponent } from '../produtos/lista/lista.component';
import { ProdutoComponent } from '../produtos/produto/produto.component';
import { VendedorComponent } from '../produtos/vendedor/vendedor.component';
import { VitrineComponent } from '../produtos/vitrine/vitrine.component';

import { ProdutosRoutingModule } from './produtos.route';
import { ProdutoService } from '../produtos/services/produtos.service';
import { FavoritosComponent } from './favoritos/favoritos.component';
import { VendedorService } from '../produtos/services/vendedor.service';
import { PaginacaoComponent } from '../components/paginacao/paginacao.component';

@NgModule({
  declarations: [
    ListaComponent,
    ProdutoComponent,
    VendedorComponent,
    VitrineComponent,
    FavoritosComponent,
    PaginacaoComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ProdutosRoutingModule,
    HttpClientModule
  ],
  providers: [
    ProdutoService,
    VendedorService
  ]
})
export class ProdutosModule { }