import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'conta',
    loadChildren: () => import('./conta/conta.module').then(mod => mod.ContaModule)
  },
  {
    path: 'produtos',
    loadChildren: () => import('./produtos/produtos.module').then(mod => mod.ProdutosModule)
  },

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
