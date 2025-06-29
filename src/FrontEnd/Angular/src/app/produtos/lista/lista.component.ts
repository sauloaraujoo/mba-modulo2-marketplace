import { Component, OnInit} from '@angular/core';
import { Produto } from '../../models/produto';
import { ProdutoService } from '../../services/produtos.service';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit {
  
  constructor(private produtoService: ProdutoService) { }

  public produtos: Produto[] = [];

  ngOnInit() {
    this.produtoService.obterProdutos()
      .subscribe(
        produtos => {
          this.produtos = produtos;
          console.log(produtos);
        },
        error => console.log(error)
      );
  }
}
