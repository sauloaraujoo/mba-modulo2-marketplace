export interface Pagined<T> {
  totalItens: number;
  paginaAtual: number;
  tamanhoPagina: number;
  itens: T[];
}