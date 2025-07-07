import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-paginacao',
  templateUrl: './paginacao.component.html',
  styleUrls: ['./paginacao.component.css']
})
export class PaginacaoComponent {
  @Input() totalItens = 0;
  @Input() paginaAtual = 1;
  @Input() tamanhoPagina = 10;

  @Output() paginaChange = new EventEmitter<number>();

  get totalPaginas(): number {
    return Math.ceil(this.totalItens / this.tamanhoPagina);
  }

  mudarPagina(pagina: number) {
    if (pagina >= 1 && pagina <= this.totalPaginas) {
      this.paginaChange.emit(pagina);
    }
  }

  gerarPaginas(): number[] {
    const paginas: number[] = [];
    const maxPaginasVisiveis = 5;
    let start = Math.max(1, this.paginaAtual - Math.floor(maxPaginasVisiveis / 2));
    let end = start + maxPaginasVisiveis - 1;

    if (end > this.totalPaginas) {
      end = this.totalPaginas;
      start = Math.max(1, end - maxPaginasVisiveis + 1);
    }

    for (let i = start; i <= end; i++) {
      paginas.push(i);
    }
    return paginas;
  }
}
