import { Component, OnInit} from '@angular/core';
import { Categoria } from '../models/categoria';
import { ProdutoService } from '../services/produtos.service';

@Component({
  selector: 'app-vitrine',
  templateUrl: './vitrine.component.html'
})
export class VitrineComponent implements OnInit {

  public categorias: Categoria[] = [];
  public categoriaSelecionada: string | undefined = undefined;

  constructor(private produtoService: ProdutoService) { }

    ngOnInit() {
      this.produtoService.obterCategorias()
        .subscribe({
          next: (categorias) => {
            this.categorias = categorias;
          },
          error: (error) => console.error(error)
        });    
    }

  onCategoriaChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    this.categoriaSelecionada = selectElement.value || undefined;
  }
}
