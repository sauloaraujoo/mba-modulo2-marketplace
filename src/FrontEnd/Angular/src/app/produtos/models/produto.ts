export interface Produto {
  id: string,
  nome: string,
  descricao: string,
  imagem: string,
  imagemUpload: string;
  preco: number,
  estoque: number,
  categoriaId: string,
  nomeCategoria: string
}