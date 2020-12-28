<h1 align="center">
     ⌨️ <a href="#" alt="Smart School WebAPI"> SmartSchoolWebAPI </a>
</h1>

<h3 align="center">
    Projeto do curso de Web API com Asp.NET Core 3.1 e Entity Framework Core 3.1  
</h3>

<h4 align="center">
	🚧   Em desenvolvimento  🚧
</h4>

## 📘 Tecnologias

- Asp.NET Core 3.1
- Entity Framework Core 3.1
- Swagger
- Docker e docker-compose

## 📋 Conceitos e observações
- Repository e IRepository: melhor encapsulamento do contexto. As classes tem acesso ao repository e o repository tem acesso ao contexto.

- AddScoped para injetar o repository na Controller: Scoped faz o .net Core criar uma intância a cada requisição e utiliza esta instância para todos objetos da requisição (escopo). Cria objetos diferentes para requisições diferentes.

- Data Transfer Object (DTO): para não retornar todas propriedades do model, é feito um DTO para a classe apenas com os objetos necessário. É possível também adicionar novos objetos, como um cálculo da idade a partir da data de nascimento, por exemplo.

- Para os campos que são iguais no model e no DTO, deve-se utilizar o AutoMapper.

- Extensions: extender classes. Pode estender uma classe DateTime, por exemplo.

- Swagger: documentação da API. Métodos, parâmetros, teste das rotas, etc. É possível fazer a documentação por versão.

- Docker-compose: arquivo as configurações para criação dos containers, imagens, networks, volumes e o que mais for necessário para a criação automática dos containers.

- Autenticação com JWT: Não faz parte do curso, mas achei importante incluir no projeto. 
