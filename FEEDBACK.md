## Funcionalidade 30%

Avalie se o projeto atende a todos os requisitos funcionais definidos.
* Ao seguir os `Passos para Execução`, nenhum produto foi exibido na loja, apesar de ter feito os migrations.
* Tentei fazer login como cliente, mas nada acontece ao clicar no botão "Login".
* Tentei criar uma nova conta, mas nada acontece ao clicar no botão "Registrar".
* A Loja faz requests para `https://localhost:7170` mas a API está rodando em `http://localhost:5032`.
* A Loja passou a listar os produtos quando executei `dotnet run -lp https` em `Api`. Mas os links da imagens estão quebrados.
* A Loja passou a exibir imagens quando executei `dotnet run -lp https` em `Mvc`.
* Login e cadadtros agora funcionam.
* Ao logar na Loja como vendedor, nenhum produto é listado, retornando "403 Forbidden"


## Qualidade do Código 20%

Considere clareza, organização e uso de padrões de codificação.

### Data
* Nomes de `DbSet` e suas tabelas devem ser no plural. Ex `Clientes`, `Produtos`, `Categorias`.
* Em `CatagoriaRepository`:
  - o nome dos paremeteros dos métodos está `Categoria entity` e deveria ser `Categoria categoria`.
  - retornar `Task<Categoria?>` quando retornar de `FirstOrDefault()`
* Os repositórios estão fazendo _dispose_ do contexto. Isso não é responsabilidade deles, e sim de quem os instancia, no caso do contêiner de injeção de dependência.
* Em `ProdutoRepository`, as chamadas a `CountAsync()` esperam um `CancellationToken`.
* Os repositórios poderiam fazer melhor uso de sobrecarga de métodos e argumentos com valores padrão. Ex:
```csharp
Task<Produto> Obter(Guid? categoriaId = null, Guid? vendedorId = null, CancellationToken cancellationToken = default);
Task<Produto[]> Listar(Guid? categoriaId = null, Guid? vendedorId = null, int pagina = 1, int tamanho = 5, CancellationToken cancellationToken = default);
```

### Business
* Bom ver entidades comportamentais. Faltou algumas validações, como duplicidade de favoritos, por exemplo.
* Em `CategoriaService` o método `Remover()` falha caso a categoria não exista, quebrando sua idempotência.
* Em `ProdutoService`, no métodos `Inserir() e `Editar()` o parametro `Produto request` deveria ser `Produto produto`.
* Em `ProdutoService.Inserir()` o primeiro `if` sempre será verdadeiro pois ele retorna `Task<T>` que sempre será diferente de `null`.

### API
* Evitar uso de "magic strings" para autorização. Ex: `[Authorize(Roles = "Administrador")]` poderia ser `[Authorize(Roles = Roles.Administrador)]` onde `Roles` é uma classe estática com constantes.
* Em `ProdutosController`, a gestão de upload de arquivos deveria ser feita em um serviço dedicado. Não é de responsabilidade da Controller.

### MVc
* Nas _controllers_, ser explícito sobre o verbo HTTP usando `[HttpGet]`, `[HttpPost]`, etc.

### Geral
* Remover códigos comentados.
* Evitar misturar idiomas (PT e EN) em nomes de variáveis, métodos e classes.
* Remove `usings` não utilizados.


## Eficiência e Desempenho 20%

Avalie o desempenho e a eficiência das soluções implementadas.
* Bom uso do token de cancelamento em métodos assíncronos.


## Inovação e Diferenciais 10%

Considere a criatividade e inovação na solução proposta.
* Bom ver adoção de versionamento de API.


## Documentação e Organização 10%

Verifique a qualidade e completude da documentação, incluindo README.md.
* Em `Executar a Aplicação Angular` faltou o comando `npm install` antes do `ng serve`.

## Resolução de Feedbacks 10%

Avalie a resolução dos problemas apontados na primeira avaliação de frontend

## Notas

| Critério                     | Peso | Nota | Nota Ponderada |
|------------------------------|------|-----:|---------------:|
| Funcionalidade               | 30%  |    9 |            2.7 |
| Qualidade do Código          | 20%  |    9 |            1.8 |
| Eficiência e Desempenho      | 20%  |    9 |            1.8 |
| Inovação e Diferenciais      | 10%  |    9 |            0.9 |
| Documentação e Organização   | 10%  |    7 |            0.7 |
| Resolução de Feedbacks       | 10%  |   10 |            1.0 |
| **Total**                    |      |      |        **8.9** |
