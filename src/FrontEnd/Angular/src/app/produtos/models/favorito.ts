import { Produto } from './produto';

export interface Favorito {
  clienteId: string;
  produtoId: string;
  produto: Produto;
  nome: string;
  imagem: string;
  preco: number;
  categoria: string;
  vendedorId: string;
  vendedor: string
}