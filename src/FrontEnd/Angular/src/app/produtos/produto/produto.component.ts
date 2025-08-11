import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Produto } from '../models/produto';
import { ActivatedRoute } from '@angular/router';
import { ProdutoService } from '../services/produtos.service';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { NotificacaoService } from 'src/app/services/notificacao.service';
@Component({
  selector: 'app-produto',
  templateUrl: './produto.component.html',
   styleUrls: ['./produto.component.css'] 
})
export class ProdutoComponent {

  @Input() produtoId?: string;

  public urlImagem: string = this.produtoService.urlImagem;
  public produto: Produto = {} as Produto;
  public id: string = "";
  private localStorageUtils = new LocalStorageUtils();
  favoritosIds = new Set<string>();
  
  constructor(private produtoService: ProdutoService, private route: ActivatedRoute, private notificacaoService: NotificacaoService) { 
    this.route.params.subscribe(res => {
    this.produtoId = res["id"];
    });
  }

  ngOnInit() {
    this.carregarProduto();
    this.carregarFavoritos();
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

  carregarFavoritos() {
  if (!this.ehUsuarioLogado()) return;

    this.produtoService.obterFavoritos().subscribe({
      next: favoritos => {
        this.favoritosIds = new Set(favoritos.map(f => f.produtoId));
      },
      error: error => console.error('Erro ao carregar favoritos:', error)
    });
  }

  ehFavorito(produtoId: string): boolean {
    return this.favoritosIds.has(produtoId);
  }

  AdicionarRemoverFavorito(produtoId: string): void {
    if (this.favoritosIds.has(produtoId)) {
      this.produtoService.removerFavorito(produtoId).subscribe({
        complete: () => {
          this.favoritosIds.delete(produtoId);
          this.favoritosIds = new Set(this.favoritosIds);

          this.notificacaoService.mostrarSucesso('Produto removido dos favoritos.');
        },
        error: (err) => {
          console.error('Erro ao remover favorito:', err);
          this.notificacaoService.mostrarErro('Erro ao remover dos favoritos.');
        }
      });
    } else {
      this.produtoService.adicionarFavorito(produtoId).subscribe({
        next: () => {
          this.favoritosIds.add(produtoId);
          this.favoritosIds = new Set(this.favoritosIds);

          this.notificacaoService.mostrarSucesso('Produto adicionado aos favoritos!');
        },
        error: (err) => {
          console.error('Erro ao adicionar favorito:', err);
          this.notificacaoService.mostrarErro('Erro ao adicionar aos favoritos.');
        }
      });
    }
  }

  ehUsuarioLogado(): boolean {
    return this.localStorageUtils.obterTokenUsuario() !== null;
  }

}
