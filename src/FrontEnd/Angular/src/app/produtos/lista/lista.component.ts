import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { Produto } from '../models/produto';
import { ProdutoService } from '../services/produtos.service';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { NotificacaoService } from 'src/app/services/notificacao.service';

@Component({
  selector: 'app-lista',
  templateUrl: './lista.component.html',
  styleUrls: ['./lista.component.css']
})
export class ListaComponent implements OnInit, OnChanges  {
  
  
  @Input() categoriaId?: string;
  @Input() vendedorId?: string;
  @Input() contexto: 'produtos' | 'favoritos' | 'vendedor' = 'produtos';

  constructor(private produtoService: ProdutoService, private notificacaoService: NotificacaoService) { }

  paginaAtual = 1;
  tamanhoPagina = 3;
  totalItens = 0;

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

      this.produtoService.obterFavoritosPaginado(this.paginaAtual, this.tamanhoPagina).subscribe({
        next: (favoritos) => {
          this.favoritosIds = new Set(favoritos.itens.map(f => f.produtoId));

          this.produtos = favoritos.itens.map(f => ({
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
          this.totalItens = favoritos.totalItens;
        },
        error: (error) => console.error('Erro ao carregar favoritos:', error)
      });
    } else if (this.contexto === 'vendedor') {
        if (this.ehUsuarioLogado()) {
          this.produtoService.obterFavoritos().subscribe({
            next: (favoritos) => {
              this.favoritosIds = new Set(favoritos.map(f => f.produtoId));

              this.produtoService.obterProdutosPorVendedorPaginado(this.paginaAtual, this.tamanhoPagina, this.vendedorId).subscribe({
                next: (res) => {
                  this.produtos = res.itens;
                  this.totalItens = res.totalItens;
                },
                error: (error) => console.error('Erro ao carregar produtos do vendedor:', error)
              });
            },
            error: (error) => console.error('Erro ao carregar favoritos:', error)
          });
        } else {
          this.produtoService.obterProdutosPorVendedorPaginado(this.paginaAtual, this.tamanhoPagina, this.vendedorId).subscribe({
            next: (res) => {
              this.produtos = res.itens;
              this.totalItens = res.totalItens;
            },
            error: (error) => console.error('Erro ao carregar produtos do vendedor:', error)
          });
        }   
    } else {
      
      if(this.ehUsuarioLogado()) {
        this.produtoService.obterFavoritos().subscribe({
          next: (favoritos) => {
            this.favoritosIds = new Set(favoritos.map(f => f.produtoId));

            this.produtoService.obterProdutosPaginado(this.paginaAtual, this.tamanhoPagina, this.categoriaId).subscribe({
              next: (produtos) => {
                this.produtos = produtos.itens;
                this.totalItens = produtos.totalItens;
              },
              error: (error) => console.error('Erro ao carregar produtos:', error)
            });
          },
          error: (error) => console.error('Erro ao carregar favoritos:', error)
        }); 
      } else {
        this.produtoService.obterProdutosPaginado(this.paginaAtual, this.tamanhoPagina, this.categoriaId).subscribe({
          next: (produtos) => {
            this.produtos = produtos.itens;
            this.totalItens = produtos.totalItens;
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
            
            if(this.produtos.length === 0) {
              this.mudarPagina(this.paginaAtual - 1);
            }
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

  mudarPagina(novaPagina: number) {
    this.paginaAtual = novaPagina;
    this.carregarProdutos();
  }
}
