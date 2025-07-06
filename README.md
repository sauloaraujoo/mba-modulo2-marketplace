# **[LojaVirtual] - Aplicação de uma plataforma de Marketplace com MVC, Angular e API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **[LojaVirtual]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Desenvolvimento Full-Stack Avançado com ASP.NET Core**.
O objetivo principal é desenvolver uma plataforma de comércio eletrônico, na qual cada vendedor adiciona e edita seus produtos, e os clientes finais visualizam todos os produtos ofertados, podendo marcar seus favoritos. A plataforma contará também com um painel administrativo. A solução para os vendedores e administradores será implementada em MVC, enquanto a vitrine para os cliente será uma SPA em Angular. Ambas soluções estarão integradas por uma API REST.

### **Autor(es)**
- **Diego Junqueira**
- **Felício Melloni**
- **Fernando Texas**
- **Márcio Gomes**
- **Renato Carrasco**
- **Rinaldo Serra**
- **Saulo Araújo**

## **2. Proposta do Projeto**

O projeto consiste em:

- **Aplicação MVC:** Interface web para administração do marketplace, onde vendedores gerenciam seus produtos e o administrador geral modera todos os cadastros.
- **Aplicação Angular:** Interface web para os clientes finais conhecerem os produtos oferecidos no marketplace e marcar seus favoritos.
- **API RESTful:** Exposição dos recursos do marketplace para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Acesso a Dados:** Implementação da persistência de dados através de ORM.
- **Negócios:** Definição dos modelos de domínio e das regras de negócio.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core MVC
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQLite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Front-end:**
  - Razor Pages/Views
  - Angular
  - HTML/CSS e Bootstrap para estilização
  - JQuery para requisições AJAX
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- src/
  - BackEnd/
    - LojaVirtual.Api/       - API RESTful
    - LojaVirtual.Business/  - Regras de negócio
    - LojaVirtual.Data/      - Persistência de dados
    - LojaVirtual.Mvc/       - Interface web administrativa
  - FrontEnd/
    - LojaVirtual.Angular    - Interface web da vitrine

## **5. Funcionalidades Implementadas**

- **Autocadastro de Vendedores:** Possibilita aos vendedores se registrarem na área administrativa do marketplace.
- **API RESTful:** Exposição de endpoints para operações CRUD, via API.
- **Documentação da API:** Documentação automática dos endpoints da API, utilizando Swagger.
- **Autocadastro de Clientes:** Possibilita aos clientes se registrarem na área de vitrine do marketplace.
- **Autenticação e Autorização:** Somente usuários autenticados poderão realizar determinadas operações, a depender de autorizações específicas para cada tipo de usuário.
- **CRUD de Categorias de Produtos:** Possibilita ao administrador criar, editar, visualizar e excluir categorias de produtos.
- **CRUD de Produtos:** Permite ao vendedor incluir, editar, visualizar e excluir seus produtos.
- **Desativação e Ativação de Vendedores e Produtos:** Dá poder de moderação ao administrador do marketplace.
- **Produtos Favoritos:** Os clientes podem manter uma lista de seus produtos favoritos.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/sauloaraujoo/mba-modulo2-marketplace.git`
   - `cd mba-modulo2-marketplace`

*** Obs: até esta linha foi alterado (carece de revisão). Abaixo desta linha, ainda será alterado. ***

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
