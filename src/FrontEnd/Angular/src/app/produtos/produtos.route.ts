import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VitrineComponent } from '../produtos/vitrine/vitrine.component';
import { ProdutoComponent } from '../produtos/produto/produto.component';
import { VendedorComponent } from '../produtos/vendedor/vendedor.component';
import { FavoritosComponent } from '../produtos/favoritos/favoritos.component';

const produtosRouterConfig: Routes = [
  { path: '', component: VitrineComponent }, // /produtos
  { 
    path: 'produto/:id', component: ProdutoComponent
  },
  { path: 'vendedor/:id', component: VendedorComponent }, // /produtos/vendedor/:id
  { path: 'favoritos', component: FavoritosComponent } // /produtos/favoritos
];

@NgModule({
    imports: [
        RouterModule.forChild(produtosRouterConfig)
    ],
    
    exports: [RouterModule]
})
export class ProdutosRoutingModule { }