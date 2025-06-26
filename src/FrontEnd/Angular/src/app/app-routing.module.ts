import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ListaComponent } from './produtos/lista/lista.component';
import { ProdutoComponent } from './produtos/produto/produto.component';
import { FavoritosComponent } from './produtos/favoritos/favoritos.component';

const routes: Routes = [
  {
    path: 'conta',
    loadChildren: () => import('./conta/conta.module').then(mod => mod.ContaModule)
  },
  { path: 'produtos', component: ListaComponent },
  { path: 'produto', component: ProdutoComponent },
  { path: 'favoritos', component: FavoritosComponent },

  // Redirecionamento opcional da raiz para login
  { path: '', redirectTo: 'produtos', pathMatch: 'full' },

  // Rota curinga (404)
  { path: '**', redirectTo: 'produtos' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
