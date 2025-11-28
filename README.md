# Kedu - Teste Técnico | Sistema de Gestão Financeira Educacional

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-13+-green)

## Sobre o Teste Técnico

Este é um **teste técnico** desenvolvido para demonstrar conhecimentos em .NET 8, Clean Architecture e desenvolvimento de APIs REST. O projeto simula uma plataforma de gestão financeira voltada ao setor educacional.

## Objetivo do Desafio

Desenvolver uma aplicação para gerenciar planos de pagamento de responsáveis financeiros

## Arquitetura

O projeto segue os princípios de **Clean Architecture** com as seguintes camadas:

```
📁 Kedu/
├── 📁 Kedu/                    Web API (Controllers, Program.cs)
├── 📁 Kedu.Domain/             Entidades, Enums, Value Objects
├── 📁 Kedu.Application/        Services, CQRS, DTOs, Interfaces
├── 📁 Kedu.Infra/              DbContext, Repositories, Migrations
└── 📁 Kedu.Test/               Testes unitários e integração
```

### Padrões Implementados

- **Repository Pattern** para acesso a dados
- **Service Layer** para lógica de negócio
- **CQRS** com MediatR para comandos e consultas
- **Dependency Injection** para inversão de controle
- **AutoMapper** para mapeamento de objetos
- **Clean Architecture** para o padrão de design

## Tecnologias

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM
- **PostgreSQL** - Banco de dados
- **MediatR** - Mediator pattern (CQRS)
- **AutoMapper** - Mapeamento de objetos
- **FluentValidation** - Validações
- **Swagger** - Documentação da API

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL 13+](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

## Instalação e Configuração

### 1. Clonar o Repositório

```bash
git clone https://github.com/CarlosHp1996/Kedu-Test.git
cd Kedu-Test
```

### 2. Configurar Banco de Dados

1. Crie um banco PostgreSQL:
```sql
CREATE DATABASE kedu_db;
```

2. Configure a connection string no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=kedu_db;Username=seu_usuario;Password=sua_senha"
  }
}
```

### 3. Executar Migrations

```bash
cd Kedu
dotnet ef database update --project ../Kedu.Infra --startup-project .
```

### 4. Executar a Aplicação

- Execute a aplicação
- A API estará disponível em: `https://localhost:7266/swagger/index.html`

## Testando a API

### Usando Swagger UI (Recomendado)

1. Acesse `https://localhost:7266/swagger/index.html`
2. Use a interface para testar os endpoints
3. Todos os endpoints estão documentados com exemplos

### Usando cURL ou Postman

Veja os exemplos detalhados na seção [Exemplos de Uso](#-exemplos-de-uso) abaixo.

## Endpoints Sugeridos

### Responsáveis Financeiros

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/v1/responsaveis` | Criar responsável financeiro |
| `GET` | `/api/v1/responsaveis/{id}` | Obter responsável por ID |
| `GET` | `/api/v1/responsaveis` | Listar todos os responsáveis |

### Planos de Pagamento

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/v1/planos-de-pagamento` | Criar plano de pagamento |
| `GET` | `/api/v1/planos-de-pagamento/{id}` | Detalhes do plano |
| `GET` | `/api/v1/planos-de-pagamento/{id}/total` | Valor total do plano |
| `GET` | `/api/v1/responsaveis/{id}/planos-de-pagamento` | Planos do responsável |

### Cobranças

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `GET` | `/api/v1/responsaveis/{id}/cobrancas` | Cobranças do responsável |
| `GET` | `/api/v1/responsaveis/{id}/cobrancas/quantidade` | Quantidade de cobranças |
| `GET` | `/api/v1/cobrancas/{id}` | Detalhes da cobrança |
| `PUT` | `/api/v1/cobrancas/{id}` | Atualizar cobrança (apenas PENDENTE) |

### Pagamentos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/v1/cobrancas/{id}/pagamentos` | **🎯 Registrar pagamento** |
| `GET` | `/api/v1/pagamentos/{id}` | Detalhes do pagamento |

## Endpoints Diferenciais (Plus)

### Centros de Custo Customizáveis

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| `POST` | `/api/v1/centros-de-custo` | Criar centro de custo |
| `GET` | `/api/v1/centros-de-custo` | Listar centros de custo |
| `PUT` | `/api/v1/centros-de-custo/{id}` | Atualizar centro de custo |
| `DELETE` | `/api/v1/centros-de-custo/{id}` | Remover centro de custo |

## 📋 Regras de Negócio

### Status da Cobrança

#### Armazenados no banco:
- **EMITIDA** - Cobrança criada e em aberto
- **PAGA** - Cobrança quitada
- **CANCELADA** - Cobrança cancelada

#### Calculado dinamicamente:
- **VENCIDA** - Calculada quando `data atual > data vencimento` e status ≠ PAGA/CANCELADA

### Regras de Pagamento

- ✅ Registrar pagamento altera status para **PAGA**
- ❌ Não é permitido pagar cobrança **CANCELADA**
- ✅ Considera pagamento total (valor exato)

### Métodos de Pagamento

- **BOLETO** - Gera código: `BOL{timestamp}{id}`
- **PIX** - Gera código: `PIX{guid}`

## Exemplos de Uso

### 1. Setup Inicial - Criar Centro de Custo

```bash
curl -X POST "https://localhost:7266/api/v1/centros-de-custo" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Mensalidade"
  }'
```

**Resposta:**
```json
{
  "value": {
    "id": 1,
    "nome": "Mensalidade"
  },
  "hasSuccess": true,
  "message": "Centro de custo criado com sucesso"
}
```

### 2. Criar Responsável Financeiro

```bash
curl -X POST "https://localhost:7266/api/v1/responsaveis" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Maria Silva"
  }'
```

**Resposta:**
```json
{
  "value": {
    "id": 1,
    "nome": "Maria Silva"
  },
  "hasSuccess": true,
  "message": "Responsável financeiro criado com sucesso"
}
```

### 3. Criar Plano de Pagamento

```bash
curl -X POST "https://localhost:7266/api/v1/planos-de-pagamento" \
  -H "Content-Type: application/json" \
  -d '{
    "responsavelId": 1,
    "centroDeCusto": 1,
    "cobrancas": [
      {
        "valor": 500.00,
        "dataVencimento": "2024-12-15T00:00:00",
        "metodoPagamento": 1
      },
      {
        "valor": 500.00,
        "dataVencimento": "2025-01-15T00:00:00",
        "metodoPagamento": 2
      }
    ]
  }'
```

**Resposta:**
```json
{
  "value": {
    "id": 1,
    "responsavelFinanceiroId": 1,
    "responsavelFinanceiroNome": "Maria Silva",
    "centroDeCustoId": 1,
    "centroDeCustoNome": "Mensalidade",
    "valorTotal": 1000.00,
    "quantidadeCobrancas": 2,
    "cobrancas": [...]
  },
  "hasSuccess": true
}
```

### 4. Registrar Pagamento (OBRIGATÓRIO)

```bash
curl -X POST "https://localhost:7266/api/v1/cobrancas/1/pagamentos" \
  -H "Content-Type: application/json" \
  -d '{
    "valor": 500.00,
    "dataPagamento": "2024-11-28T10:30:00"
  }'
```

**Resposta:**
```json
{
  "value": {
    "pagamentoId": 1,
    "cobrancaId": 1,
    "novoStatusCobranca": 2,
    "mensagem": "Pagamento processado com sucesso",
    "sucesso": true
  },
  "hasSuccess": true
}
```

### 5. Consultar Cobranças do Responsável

```bash
curl -X GET "https://localhost:7266/api/v1/responsaveis/1/cobrancas"
```

**Resposta:**
```json
{
  "value": {
    "cobrancas": [
      {
        "id": 1,
        "planoDePagamentoId": 1,
        "valor": 500.00,
        "dataVencimento": "2024-12-15T00:00:00",
        "metodoPagamento": 1,
        "metodoPagamentoDescricao": "Boleto Bancário",
        "status": 2,
        "statusDescricao": "Paga",
        "codigoPagamento": "BOL20241128103000000001",
        "isVencida": false,
        "pagamentos": [...]
      }
    ]
  },
  "hasSuccess": true
}
```

### 6. Consultar Total do Plano

```bash
curl -X GET "https://localhost:7266/api/v1/planos-de-pagamento/1/total"
```

**Resposta:**
```json
{
  "value": 1000.00,
  "hasSuccess": true,
  "message": "Total calculado com sucesso"
}
```

### 7. Atualizar Cobrança (Parcial)

```bash
curl -X PUT "https://localhost:7266/api/v1/cobrancas/2" \
  -H "Content-Type: application/json" \
  -d '{
    "valor": 550.00
  }'
```

> ✅ **Atualização Parcial**: Só atualiza os campos enviados, mantém os demais valores originais.

## Estrutura do Projeto

### Entidades Principais

```
ResponsavelFinanceiro
├── Id: int
└── Nome: string

CentroDeCusto  
├── Id: int
└── Nome: string

PlanoDePagamento
├── Id: int
├── ResponsavelFinanceiroId: int
├── CentroDeCustoId: int
└── ValorTotal: decimal (calculado)

Cobranca
├── Id: int
├── PlanoDePagamentoId: int
├── Valor: decimal
├── DataVencimento: DateTime
├── MetodoPagamento: enum
├── Status: enum
└── CodigoPagamento: string

Pagamento
├── Id: int
├── CobrancaId: int
├── Valor: decimal
└── DataPagamento: DateTime
```

### Enums

```csharp
// MetodoPagamento
public enum MetodoPagamento
{
    Boleto = 1,
    Pix = 2
}

// StatusCobranca
public enum StatusCobranca
{
    Emitida = 1,
    Paga = 2,
    Cancelada = 3
}
```
---
*Este projeto foi desenvolvido como parte de um processo seletivo, demonstrando conhecimentos em .NET 8, Clean Architecture, Entity Framework, PostgreSQL e boas práticas de desenvolvimento.*
