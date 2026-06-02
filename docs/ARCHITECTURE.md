# ARCHITECTURE.md

# AI Travel Planner

## Overview

AI Travel Planner é uma aplicação web full-stack desenvolvida em .NET utilizando Blazor Server para interface e backend, SQLite para persistência de dados e Gemini como provedor de Inteligência Artificial para geração dos planos de viagem.

O objetivo desta arquitetura é maximizar velocidade de desenvolvimento, simplicidade operacional e baixo custo de infraestrutura para validação do MVP.

A interface será desenvolvida seguindo a abordagem **Mobile First**, priorizando a experiência em dispositivos móveis e adaptando-se progressivamente para tablets e desktops.

---

# Architecture Principles

## Simplicidade

Priorizar soluções simples sobre arquiteturas complexas.

## Baixo Custo

Permitir execução em VPS de baixo custo.

## Fácil Evolução

Estrutura preparada para futura migração para PostgreSQL e múltiplos provedores de IA.

## Monolito Modular

Todo o sistema será desenvolvido em uma única aplicação.

Não haverá microsserviços no MVP.

## Mobile First

Toda a experiência do usuário será projetada inicialmente para dispositivos móveis.

Layouts, componentes e fluxos deverão funcionar perfeitamente em telas pequenas antes da adaptação para telas maiores.

## Domain Driven Organization

A aplicação será organizada por funcionalidades (Features), mantendo cada módulo responsável por seu próprio domínio, serviços, componentes e persistência.

O objetivo é reduzir acoplamento e facilitar evolução futura sem necessidade de migrar para uma arquitetura distribuída.

---

# High Level Architecture

```text
┌─────────────────────┐
│     Blazor UI       │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Application Layer   │
│ Use Cases           │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Domain Layer        │
│ Business Rules      │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Infrastructure      │
│ EF Core             │
│ Gemini API          │
│ Logging             │
└─────────────────────┘
```

---

# Architectural Layers

## UI

Responsável pela interação com o usuário.

Contém:

* Pages
* Components
* Layouts

Não deve conter regras de negócio.

---

## Application

Responsável pelos casos de uso da aplicação.

Exemplos:

* Criar viagem
* Atualizar viagem
* Excluir viagem
* Gerar plano
* Regenerar plano

---

## Domain

Responsável pelas regras de negócio.

Exemplos:

* Validação de viagens
* Regras de orçamento
* Regras de geração de planos

---

## Infrastructure

Responsável por integrações externas.

Exemplos:

* Entity Framework Core
* SQLite
* Gemini
* Logging

---

# Technology Stack

## Backend

* .NET 10
* ASP.NET Core
* Blazor Server

## Frontend

* Blazor Components
* CSS
* HTML
* Responsive Design
* Mobile First

## Database

* SQLite

## ORM

* Entity Framework Core

## AI Provider

* Google Gemini

## Authentication

Não haverá autenticação completa no MVP.

Será utilizada proteção mínima de acesso para evitar uso indevido da API Gemini.

## Logging

* Microsoft Logging

## Hosting

* Linux VPS
* Docker

---

# Solution Structure

```text
src/

AITravelPlanner.Web/

├── Features/
│
├── Trips/
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   └── UI/
│
├── TravelPlans/
│   ├── Domain/
│   ├── Application/
│   ├── Infrastructure/
│   └── UI/
│
├── Shared/
│
├── Infrastructure/
│   ├── Persistence/
│   ├── AI/
│   ├── Logging/
│   └── Security/
│
└── Program.cs
```

---

# Feature Organization

## Dashboard

Responsável pela listagem de viagens.

### Funcionalidades

* Listar viagens
* Exibir resumo

---

## Trips

Responsável pelo gerenciamento das viagens.

### Funcionalidades

* Criar viagem
* Editar viagem
* Excluir viagem
* Visualizar viagem

---

## TravelPlans

Responsável pelo gerenciamento dos planos gerados pela IA.

### Funcionalidades

* Gerar plano
* Regenerar plano
* Visualizar plano
* Histórico de gerações

---

# UI/UX Guidelines

## Mobile First

A interface deverá ser construída inicialmente para smartphones.

### Diretrizes

* Navegação simples e intuitiva
* Formulários otimizados para toque
* Botões com área mínima adequada para dispositivos móveis
* Layout em coluna única por padrão
* Componentes responsivos
* Uso mínimo de modais
* Conteúdo legível sem zoom

### Breakpoints

```text
Mobile: até 767px
Tablet: 768px até 1023px
Desktop: acima de 1024px
```

---

# Domain Models

## Trip

```csharp
Trip
{
    Guid Id;

    string Destination;

    string Country;

    DateOnly StartDate;
    DateOnly EndDate;

    int NumberOfPeople;

    decimal Budget;

    string Objectives;

    string AdditionalNotes;

    DateTime CreatedAt;
}
```

---

## TravelPlan

```csharp
TravelPlan
{
    Guid Id;

    Guid TripId;

    string Prompt;

    string GeneratedContent;

    string PromptVersion;

    string AiProvider;

    string Model;

    int? TokensUsed;

    decimal? EstimatedCost;

    PlanStatus Status;

    DateTime GeneratedAt;
}
```

---

## PlanStatus

```csharp
public enum PlanStatus
{
    Pending,
    Processing,
    Completed,
    Failed
}
```

---

# Database Schema

## Trips

```text
Id
Destination
Country
StartDate
EndDate
NumberOfPeople
Budget
Objectives
AdditionalNotes
CreatedAt
```

## TravelPlans

```text
Id
TripId
Prompt
GeneratedContent
PromptVersion
AiProvider
Model
TokensUsed
EstimatedCost
Status
GeneratedAt
```

---

# Database Configuration

A aplicação deverá utilizar Entity Framework Core com configuração baseada em Connection String.

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  }
}
```

Essa abordagem permitirá futura migração para PostgreSQL apenas alterando a configuração de infraestrutura.

---

# Repositories

A camada Application não deverá acessar diretamente o Entity Framework.

Serão utilizadas interfaces para abstração da persistência.

## ITripRepository

```csharp
public interface ITripRepository
{
    Task<Trip?> GetByIdAsync(Guid id);
    Task<List<Trip>> GetAllAsync();
    Task CreateAsync(Trip trip);
    Task UpdateAsync(Trip trip);
    Task DeleteAsync(Guid id);
}
```

---

## ITravelPlanRepository

```csharp
public interface ITravelPlanRepository
{
    Task<TravelPlan?> GetByIdAsync(Guid id);
    Task<List<TravelPlan>> GetByTripAsync(Guid tripId);
    Task CreateAsync(TravelPlan plan);
}
```

---

# AI Integration

## Goal

Gerar um plano de viagem completo baseado nos dados fornecidos pelo usuário.

---

## Prompt Strategy

O backend será responsável por construir um prompt estruturado.

Exemplo:

```text
Crie um plano de viagem completo.

Destino: Lisboa
País: Portugal
Período: 10 dias
Pessoas: 2
Orçamento: R$ 10.000

Objetivo:
Conhecer pontos turísticos e gastronomia.

Retorne:

- Resumo
- Custos estimados
- Roteiro diário
- Cuidados
- Dicas locais
```

---

## Output Format

Inicialmente será utilizado Markdown.

Exemplo:

```markdown
# Resumo

...

# Custos

...

# Roteiro Diário

...
```

---

# AI Processing Flow

A geração de planos seguirá o fluxo abaixo:

```text
Usuário cria viagem
        │
        ▼
Solicita geração
        │
        ▼
TravelPlan(Status = Pending)
        │
        ▼
GeminiService
        │
        ▼
Processamento
        │
        ▼
TravelPlan(Status = Completed)
```

Caso ocorra erro:

```text
TravelPlan(Status = Failed)
```

Esta estrutura permite futura migração para processamento assíncrono utilizando Background Jobs sem alterações significativas no domínio.

---

# Application Services

## TripService

Responsável por:

* Criar viagem
* Atualizar viagem
* Buscar viagem
* Excluir viagem

---

## TravelPlanService

Responsável por:

* Gerar plano
* Consultar plano
* Regenerar plano

---

## GeminiService

Responsável por:

* Comunicação com Gemini
* Envio de prompts
* Tratamento de respostas

---

# Authentication

Não haverá autenticação completa na versão MVP.

Justificativa:

O objetivo inicial é validar a geração de roteiros utilizando IA com o menor nível possível de complexidade.

Será implementado apenas um mecanismo simples de restrição de acesso para evitar consumo indevido da API Gemini.

---

# Error Handling

## Regras

Toda chamada para IA deve:

* Registrar logs
* Capturar exceções
* Atualizar status do plano
* Retornar mensagens amigáveis

Exemplo:

```text
Não foi possível gerar o plano de viagem.
Tente novamente em alguns instantes.
```

---

# Observability

## Logging

Será utilizado logging estruturado.

Tecnologia inicial:

* Microsoft Logging

Tecnologia futura:

* Serilog
* Seq

Eventos registrados:

* Criação de viagem
* Atualização de viagem
* Exclusão de viagem
* Geração de plano
* Falhas de integração com IA

Não registrar:

* Dados sensíveis
* Prompts contendo informações privadas
* API Keys
* Tokens

---

# Security

## MVP

* HTTPS obrigatório
* CSRF Protection
* Input Validation
* Secret Management
* Rate Limiting básico
* Restrição de acesso administrativo
* Proteção contra exposição de segredos

Mesmo sem autenticação completa, o sistema deverá possuir proteção mínima contra uso público irrestrito.

---

# Deployment

## Container

```text
Docker
```

---

## Infraestrutura

```text
Ubuntu Server

Docker

AI Travel Planner
SQLite
```

---

# Backup Strategy

## SQLite

Backup diário do arquivo:

```text
app.db
```

Armazenar:

```text
backups/
```

---

# Future Evolution

## V2

* PostgreSQL
* Cache
* OpenAI
* Claude
* Autenticação de usuários
* Background Jobs

---

## V3

* APIs de hotéis
* APIs de voos
* APIs de clima

---

## V4

* Aplicativo Mobile
* Planejamento colaborativo
* Compartilhamento de viagens

---

# Non-Functional Requirements

## Performance

* Tempo de resposta da UI inferior a 2 segundos
* Geração de plano inferior a 60 segundos

## Availability

* 99% de disponibilidade

## Scalability

* Suporte inicial para até 1.000 usuários

## Maintainability

* Organização por feature
* Separação entre UI, Application, Domain e Infrastructure
* Repositórios desacoplados da aplicação

## Responsiveness

* Experiência otimizada para dispositivos móveis
* Compatibilidade com tablets e desktops
* Layout adaptável sem perda de funcionalidade

---

# MVP Definition

O MVP será considerado concluído quando permitir:

1. Criação de viagem
2. Edição de viagem
3. Exclusão de viagem
4. Geração de plano usando Gemini
5. Salvamento do plano
6. Consulta de viagens anteriores
7. Regeneração do plano
8. Controle de status da geração

---

## Não faz parte do MVP

* Cadastro de usuário
* Login
* Logout
* OAuth
* Compartilhamento de viagens
* Integrações externas
* Aplicativo Mobile
* Processamento distribuído

Qualquer funcionalidade além desta lista pertence às próximas versões.
