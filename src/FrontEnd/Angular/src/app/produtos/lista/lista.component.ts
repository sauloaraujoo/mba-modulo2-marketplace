import { Component, Input, OnInit} from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produtos.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit {
  
  @Input() categoriaId?: string;
  @Input() vendedorId?: string;
  @Input() contexto: 'produtos' | 'favoritos' | 'vendedor' = 'produtos';

  constructor(private produtoService: ProdutoService) { }

  public produtos: Produto[] = [];

  ngOnInit() {

    if (this.contexto === 'favoritos') {
      //definir parte da pagina de favoritos
    } else if (this.vendedorId) {
      //definir parte da pagina de detalhes do vendedor
    } else {
      this.produtoService.obterProdutos(this.categoriaId)
        .subscribe({
          next: (produtos) => {
            this.produtos = produtos;
            console.log(produtos);
          },
          error: (error) => console.error(error)
        });    
      }
  }
}
