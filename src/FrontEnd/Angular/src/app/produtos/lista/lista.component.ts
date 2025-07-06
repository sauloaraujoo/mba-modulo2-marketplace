import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produtos.service';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { NotificacaoService } from 'src/app/services/notificacao.service';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html'
})
export class ListaComponent implements OnInit, OnChanges  {
  
  
  @Input() categoriaId?: string;
  @Input() vendedorId?: string;
  @Input() contexto: 'produtos' | 'favoritos' | 'vendedor' = 'produtos';

  constructor(private produtoService: ProdutoService, private notificacaoService: NotificacaoService) { }

  public urlImagem: string = this.produtoService.urlImagem;
  public produtos: Produto[] = [];
  public favoritosIds: Set<string> = new Set();
  public mensagem: string | null = null;
  public mensagemTipo: 'success' | 'danger' | 'info' | null = null;
  private localStorageUtils = new LocalStorageUtils();

  ngOnInit() {
    this.carregarProdutos();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['categoriaId'] && !changes['categoriaId'].firstChange) {
      this.carregarProdutos();
    }
  }  

  ehUsuarioLogado(): boolean {
    return this.localStorageUtils.obterTokenUsuario() !== null;
  }

  private carregarProdutos(): void {

    if (this.contexto === 'favoritos') {
      if (!this.ehUsuarioLogado()) {
        this.produtos = [];
        return;
      }

      this.produtoService.obterFavoritos().subscribe({
        next: (favoritos) => {
          this.favoritosIds = new Set(favoritos.map(f => f.produtoId));

          this.produtos = favoritos.map(f => ({
            id: f.produtoId,
            nome: f.nome,
            imagem: f.imagem,
            imagemUpload: '',
            preco: f.preco,
            categoriaId: '', 
            nomeCategoria: f.categoria ?? '',
            descricao: '',
            estoque: 0,
            vendedorId: f.vendedorId ?? '',
            nomeVendedor: f.vendedor ?? ''
          }));
        },
        error: (error) => console.error('Erro ao carregar favoritos:', error)
      });
    } else if (this.contexto === 'vendedor') {
      this.produtoService.obterProdutosPorVendedor(this.vendedorId)
        .subscribe({
          next: (produtos) => {
            this.produtos = produtos;
            console.log(produtos);
          },
          error: (error) => console.error(error)
        });    
    } else {
      if(this.ehUsuarioLogado()) {
        this.produtoService.obterFavoritos().subscribe({
          next: (favoritos) => {
            this.favoritosIds = new Set(favoritos.map(f => f.produtoId));

            this.produtoService.obterProdutos(this.categoriaId).subscribe({
              next: (produtos) => {
                this.produtos = produtos;
              },
              error: (error) => console.error('Erro ao carregar produtos:', error)
            });
          },
          error: (error) => console.error('Erro ao carregar favoritos:', error)
        }); 
      } else {
        this.produtoService.obterProdutos(this.categoriaId).subscribe({
          next: (produtos) => {
            this.produtos = produtos;
          },
          error: (error) => console.error('Erro ao carregar produtos:', error)
        });
      }        
    }      
  }
  
  AdicionarRemoverFavorito(produtoId: string): void {
    if (this.favoritosIds.has(produtoId)) {
      this.produtoService.removerFavorito(produtoId).subscribe({
        complete: () => {
          this.favoritosIds.delete(produtoId);
          this.favoritosIds = new Set(this.favoritosIds); 

          if (this.contexto === 'favoritos') {
            this.produtos = this.produtos.filter(p => p.id !== produtoId);
          }
          this.notificacaoService.showSuccess('Produto removido dos favoritos.');
        },
        error: (err) => {
          console.error('Erro ao remover favorito:', err);
          this.notificacaoService.showError('Erro ao remover dos favoritos.');
        } 
      });
    } else {
      this.produtoService.adicionarFavorito(produtoId).subscribe({
        next: () => {
          this.favoritosIds.add(produtoId);
          this.favoritosIds = new Set(this.favoritosIds); 

          this.notificacaoService.showSuccess('Produto adicionado aos favoritos!');
        },
        error: (err) => {
          console.error('Erro ao adicionar favorito:', err)
          this.notificacaoService.showError('Erro ao adicionar aos favoritos.');
        }
      });
    }
  }

  ehFavorito(produtoId: string): boolean {
    return this.favoritosIds.has(produtoId);
  }
}
