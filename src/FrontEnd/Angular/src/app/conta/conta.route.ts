import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContaAppComponent } from './conta.app.component';
import { LoginComponent } from './login/login.component';
import { CadastroClienteComponent } from './cadastro-cliente/cadastro-cliente.component';
import { EditarClienteComponent } from './editar-cliente/editar-cliente.component';

import { ContaGuard } from './services/conta.guard';

const contaRouterConfig: Routes = [
    {
        path: '', component: ContaAppComponent,
        children: [
            { path: 'cadastro', component: CadastroClienteComponent, canActivate: [ContaGuard], canDeactivate: [ContaGuard] },
            { path: 'login', component: LoginComponent, canActivate: [ContaGuard] },
            { path: 'edicao-cliente', component: EditarClienteComponent,  },
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(contaRouterConfig)
    ],
    exports: [RouterModule]
})
export class ContaRoutingModule { }