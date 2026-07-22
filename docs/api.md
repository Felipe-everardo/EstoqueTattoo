# API

Resumo dos principais endpoints da aplicação.

## Materiais

```txt
GET    /api/materiais
GET    /api/materiais/{id}
GET    /api/materiais/criticos
POST   /api/materiais
DELETE /api/materiais/{id}
```

Exemplo de criação:

```json
{
  "nome": "Tinta Preta Dynamic 240ml",
  "categoriaId": 2,
  "quantidadeAtual": 3,
  "quantidadeMinima": 1,
  "precoUnitario": 190.00
}
```

## Movimentações

```txt
GET  /api/movimentacoes
GET  /api/movimentacoes/{id}
POST /api/movimentacoes
```

Exemplo de movimentação:

```json
{
  "materialId": 1,
  "quantidade": 2,
  "tipo": "Saida",
  "observacao": "Baixa feita em materiais"
}
```

## Tintas em Uso

```txt
GET    /api/tintas
POST   /api/tintas
PUT    /api/tintas/{id}/nivel
DELETE /api/tintas/{id}
```

Exemplo para abrir um item na bancada:

```json
{
  "materialId": 1
}
```

Exemplo para atualizar o nível:

```json
{
  "novaPorcentagem": 50
}
```

## Dashboard

```txt
GET /api/dashboard/resumo
```

Retorna totais de materiais, itens críticos, tintas em uso, tintas em alerta e últimas movimentações.

## Erros

Algumas regras de negócio retornam erros padronizados:

```json
{
  "code": "INSUFFICIENT_STOCK",
  "message": "Estoque insuficiente para realizar a saída."
}
```
