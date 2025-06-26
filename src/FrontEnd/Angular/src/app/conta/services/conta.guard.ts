import { Injectable } from '@angular/core';
import { CanDeactivate, CanActivate, Router } from '@angular/router';

import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { CadastroClienteComponent } from '../cadastro-cliente/cadastro-cliente.component';



@Injectable()
export class ContaGuard implements CanDeactivate<CadastroClienteComponent>, CanActivate {
    
    localStorageUtils = new LocalStorageUtils();

    constructor(private router: Router){}
    
    canDeactivate(component: CadastroClienteComponent) {
        if(component.mudancasNaoSalvas) {
            return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulario?');
        }  

        return true
    }

    canActivate() {
        if(this.localStorageUtils.obterTokenUsuario()){
            this.router.navigate(['/home']);
        }

        return true;
    }
    
}