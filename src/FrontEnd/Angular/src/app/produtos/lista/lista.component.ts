import { Component, OnInit} from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produtos.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit {
  
  constructor(private produtoService: ProdutoService) { }

  public produtos: Produto[] = [];

  ngOnInit() {

  this.produtoService.obterProdutos()
    .subscribe({
      next: (produtos) => {
        this.produtos = produtos;
        console.log(produtos);
      },
      error: (error) => console.error(error)
    });
  }
}
