# Lia Tattoo Art - Controle de Estoque

Sistema full-stack desenvolvido para gerenciar o estoque de materiais de um estúdio de tatuagem. A aplicação permite cadastrar materiais, controlar entradas e saídas, acompanhar tintas em uso na bancada e consultar o histórico automático de movimentações.

O projeto foi construído com React no front-end, ASP.NET Core no back-end, Entity Framework Core e SQLite, com foco em organização de código, separação de responsabilidades e uma estrutura simples para execução local.

## Tecnologias

- Backend: C#, ASP.NET Core Web API, Entity Framework Core, SQLite
- Frontend: React, Vite, Axios, React Router DOM, Bootstrap
- Banco de dados: SQLite local

## Estrutura

```txt
LiaTattooArt-monorepo/
  backend/
    EstoqueLiaTattoo.slnx
    EstoqueLiaTattoo/
  frontend/
    package.json
    src/
```

## Como Rodar o Backend

Entre na pasta do projeto da API:

```powershell
cd backend\EstoqueLiaTattoo
dotnet restore
dotnet ef database update
dotnet run --launch-profile https
```

O comando `dotnet ef database update` cria o arquivo local `Data/estoque.db` e aplica dados iniciais de categorias e materiais.

A API deve ficar disponivel em:

```txt
https://localhost:7153
http://localhost:5207
```

## Como Rodar o Frontend

Em outro terminal:

```powershell
cd frontend
npm install
npm run dev
```

Abra no navegador:

```txt
http://localhost:5173
```

## Comunicacao Frontend e Backend

O frontend chama endpoints usando `/api`. Em desenvolvimento, o Vite redireciona essas chamadas para:

```txt
https://localhost:7153
```

Exemplo:

```txt
/api/materiais -> https://localhost:7153/api/materiais
```

## Funcionalidades

- Cadastro e listagem de materiais
- Controle de quantidade atual e quantidade minima
- Categorias de materiais
- Registro de entradas e saidas de estoque
- Controle de tintas abertas na bancada
- Atualizacao do nivel restante de tintas

