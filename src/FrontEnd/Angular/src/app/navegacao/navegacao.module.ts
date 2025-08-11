import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { MenuComponent } from './menu/menu.component';

import { MenuLoginComponent } from './menu-login/menu-login.component';
import { AcessoNegadoComponent } from './acesso-negado/acesso-negado.component';
import { RodapeComponent } from "./rodape/rodape.component";

@NgModule({
    declarations: [
        MenuComponent,
        MenuLoginComponent,
        RodapeComponent,
        AcessoNegadoComponent
    ],
    imports: [
        CommonModule,
        RouterModule
    ],
    exports: [
        MenuComponent,
        MenuLoginComponent,
        RodapeComponent,
        AcessoNegadoComponent
    ]
})
export class NavegacaoModule { }