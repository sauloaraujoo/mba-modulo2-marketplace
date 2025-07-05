import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Produto } from '../models/produto';
import { Router,ActivatedRoute } from '@angular/router';
import { ProdutoService } from '../services/produtos.service';
@Component({
  selector: 'app-produto',
  templateUrl: './produto.component.html',
  styles: [
  ]
})
export class ProdutoComponent {

  @Input() produtoId?: string;

  public urlImagem: string = this.produtoService.urlImagem;
  public produto: Produto = {} as Produto;
  public id: string = "";
  


  
  constructor(private produtoService: ProdutoService, private route: ActivatedRoute) { 
    this.route.params.subscribe(res => {
    this.produtoId = res["id"];
    });
console.log(this.route.params);
  }

  ngOnInit() {
    console.log('Produto ID:', this.produtoId);
    this.carregarProduto();
  }


  private carregarProduto(): void {

  
      this.produtoService.obterProduto(this.produtoId)
        .subscribe({
          next: (produto) => {
            this.produto = produto;
            console.log(produto);
          },
          error: (error) => console.error(error)
        });    
      
      
  }  

}
