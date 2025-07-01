import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produtos.service';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit, OnChanges  {
  
  @Input() categoriaId?: string;
  @Input() vendedorId?: string;
  @Input() contexto: 'produtos' | 'favoritos' | 'vendedor' = 'produtos';

  constructor(private produtoService: ProdutoService) { }

  public urlImagem: string = this.produtoService.urlImagem;
  public produtos: Produto[] = [];

  ngOnInit() {
    this.carregarProdutos();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['categoriaId'] && !changes['categoriaId'].firstChange) {
      this.carregarProdutos();
    }
  }  

  private carregarProdutos(): void {

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
