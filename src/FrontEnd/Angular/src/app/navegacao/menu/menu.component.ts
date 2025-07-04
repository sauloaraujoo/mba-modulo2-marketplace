import { Component } from '@angular/core';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styles: [
  ]
})
export class MenuComponent {
  localStorageUtils = new LocalStorageUtils();

  usuarioLogado(): boolean {
    return this.localStorageUtils.obterTokenUsuario() !== null;
  }

}
