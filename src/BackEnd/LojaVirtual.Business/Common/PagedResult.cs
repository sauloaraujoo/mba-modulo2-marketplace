namespace LojaVirtual.Business.Common
{
    public class PagedResult<T>
    {
        public int TotalItens { get; set; }
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public IEnumerable<T> Itens { get; set; }
    }
}
