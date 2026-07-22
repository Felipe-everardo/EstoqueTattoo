# Arquitetura

O projeto usa uma estrutura full-stack simples, separando interface, API, regras de negócio e persistência.

```txt
React + Vite
    |
    | HTTP JSON via Axios
    v
ASP.NET Core Controllers
    |
    | DTOs de entrada e resposta
    v
Services
    |
    | regras de negócio e transações
    v
Entity Framework Core
    |
    v
SQLite
```

## Decisões

- Controllers recebem DTOs e traduzem resultados para HTTP.
- Services concentram regras de negócio como movimentações, baixa de estoque e abertura de tintas.
- Entidades representam o banco de dados e não são usadas como contrato principal de entrada da API.
- Movimentações são geradas automaticamente para manter histórico auditável do estoque.
- Materiais removidos são desativados com `IsAtivo`, preservando o histórico.
- O front-end usa serviços próprios em `src/services` para concentrar chamadas HTTP.

## Regras Críticas

- Uma saída não pode deixar o estoque negativo.
- Abrir uma tinta na bancada reduz o estoque em uma unidade.
- Criar material com quantidade inicial gera movimentação de entrada.
- Remover material não apaga movimentações antigas.
- Tintas com nível abaixo de 20% entram em alerta no dashboard.

## Evolução Planejada

- Adicionar autenticação.
- Expandir testes de integração.
- Publicar uma versão demonstrável.
- Adicionar filtros avançados no histórico.
