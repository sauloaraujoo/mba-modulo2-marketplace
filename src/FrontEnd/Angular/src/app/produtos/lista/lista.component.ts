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
  public favoritosIds: Set<string> = new Set();
  public mensagem: string | null = null;
  public mensagemTipo: 'success' | 'danger' | 'info' | null = null;

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
            nomeVendedor: f.vendedor ?? ''
          }));
        },
        error: (error) => console.error('Erro ao carregar favoritos:', error)
      });
    } else if (this.vendedorId) {
      //definir parte da pagina de detalhes do vendedor
    } else {
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
          this.mostrarMensagem('Produto removido dos favoritos.', 'success');
        },
        error: (err) => {
          console.error('Erro ao remover favorito:', err);
          this.mostrarMensagem('Erro ao remover dos favoritos.', 'danger');
        } 
      });
    } else {
      this.produtoService.adicionarFavorito(produtoId).subscribe({
        next: () => {
          this.favoritosIds.add(produtoId);
          this.favoritosIds = new Set(this.favoritosIds); 

          this.mostrarMensagem('Produto adicionado aos favoritos!', 'success');
        },
        error: (err) => {
          console.error('Erro ao adicionar favorito:', err)
          this.mostrarMensagem('Erro ao adicionar aos favoritos.', 'danger');
        }
      });
    }
  }

  ehFavorito(produtoId: string): boolean {
    return this.favoritosIds.has(produtoId);
  }

  private mostrarMensagem(msg: string, tipo: 'success' | 'danger' | 'info' = 'info'): void {
    this.mensagem = msg;
    this.mensagemTipo = tipo;

    setTimeout(() => {
      this.mensagem = null;
      this.mensagemTipo = null;
    }, 2000);
  }
}
