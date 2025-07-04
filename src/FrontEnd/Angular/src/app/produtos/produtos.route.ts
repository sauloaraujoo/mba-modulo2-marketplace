import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { VitrineComponent } from '../produtos/vitrine/vitrine.component';
import { ProdutoComponent } from '../produtos/produto/produto.component';
import { FavoritosComponent } from '../produtos/favoritos/favoritos.component';

const produtosRouterConfig: Routes = [
  { path: 'favoritos', component: FavoritosComponent }, // /produtos/favoritos
  { path: 'produto/:id', component: ProdutoComponent }, 
  { path: '', component: VitrineComponent } // /produtos  
];

@NgModule({
    imports: [
        RouterModule.forChild(produtosRouterConfig)
    ],
    
    exports: [RouterModule]
})
export class ProdutosRoutingModule { }