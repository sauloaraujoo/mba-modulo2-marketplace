import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VitrineComponent } from '../produtos/vitrine/vitrine.component';
import { ProdutoComponent } from '../produtos/produto/produto.component';
import { FavoritosComponent } from '../produtos/favoritos/favoritos.component';

const produtosRouterConfig: Routes = [
  { path: '', component: VitrineComponent }, // /produtos
  { path: ':id', component: ProdutoComponent }, // /produtos/:id
  { path: 'favoritos', component: FavoritosComponent } // /produtos/favoritos
];

@NgModule({
    imports: [
        RouterModule.forChild(produtosRouterConfig)
    ],
    exports: [RouterModule]
})
export class ProdutosRoutingModule { }