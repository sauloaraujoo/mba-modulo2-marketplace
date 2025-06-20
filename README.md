# **[LojaVirtual] - Aplicação de uma Mini Loja Virtual com MVC e API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **[LojaVirtual]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Desenvolvimento Full-Stack Avançado com ASP.NET Core**.
O objetivo principal é ...

### **Autor(es)**
- **Rinaldo Serra**

## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação MVC:** Interface web para interação com a loja.
- **API RESTful:** Exposição dos recursos da loja para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação:** Implementação de controle de acesso.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQLite
- **Autenticação:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Razor Pages/Views
  - HTML/CSS e Bootstrap para estilização
  - JQuery para requisições AJAX
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- src/
  - LojaVirtual.Mvc/ - Projeto MVC
  - LojaVirtual.Api/ - API RESTful
  - LojaVirtual.Core/ - Modelos de Dados, Serviços de negócios e Configuração do EF Core  
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **Registro de Usuários:** Permite incluir usuários à Loja que são inseridos também como vendedores para cadastros das categorias e produtos.
- **CRUD para Categorias:** Permite criar, editar, visualizar e excluir categorias.
- **CRUD para Produtos:** Permite criar, editar, visualizar e excluir os produtos do próprio vendedor.
- **Autenticação:** Somente usuários autenticados poderão realizar criação, edição e exclusão das categorias e produtos.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/rinaldoserra-dev/LojaVirtual-Mba.git`
   - `cd LojaVirtual-Mba`

2. **Configuração do Banco de Dados:**
   - No arquivo `appsettings.json`, configure a string de conexão do SQL Server nos projetos BlogExpert.Mvc e BlogExpert.Api.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos.

3. **Executar a Aplicação MVC:**
   - `cd src/LojaVirtual.Mvc/`
   - `dotnet run`
   - Acesse a aplicação em: http://localhost:5225/

4. **Executar a API:**
   - `cd src/LojaVirtual.Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5032/swagger/ 
   
5. **Usuários registrados na carga inicial:**
   - rinaldo@teste.com   
   - A senha para todos esses usuários é a mesma: Teste@123

## **7. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5032/swagger/ 

## **9. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
