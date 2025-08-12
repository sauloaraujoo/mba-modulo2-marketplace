import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MenuComponent } from './menu/menu.component';

import { MenuLoginComponent } from './menu-login/menu-login.component';
import { AcessoNegadoComponent } from './acesso-negado/acesso-negado.component';
import { RodapeComponent } from "./rodape/rodape.component";
import { NaoEncontradoComponent } from './nao-encontrado/nao-encontrado.component';

@NgModule({
    declarations: [
        MenuComponent,
        MenuLoginComponent,
        RodapeComponent,
        AcessoNegadoComponent,
        NaoEncontradoComponent
    ],
    imports: [
        CommonModule,
        RouterModule
    ],
    exports: [
        MenuComponent,
        MenuLoginComponent,
        RodapeComponent,
        AcessoNegadoComponent,
        NaoEncontradoComponent
    ]
})
export class NavegacaoModule { }