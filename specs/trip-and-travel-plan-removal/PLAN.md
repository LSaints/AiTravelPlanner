# PLAN.md

## Existing Context

### Architecture

Feature-based Clean Architecture (Vertical Slices)

### Stack

- ASP.NET Core com Blazor Server (interactive server rendering)
- Entity Framework Core 10.0.8
- SQLite
- xUnit + Moq (testes)

### Existing Conventions

- Repository Pattern (interface em `Application/`, implementação em `Infrastructure/Persistence/Repositories/`)
- Use Case Pattern (classe por operação com método `ExecuteAsync`)
- Request DTOs como `record` types no mesmo arquivo do use case
- Mapeamento manual entre DTO e entidade
- DI registrada em `Program.cs` como `Scoped`

### Naming Standards

- Interfaces: `I{Entity}Repository`
- Use cases: `{Operation}{Entity}` (ex.: `DeleteTrip`, `CreateTrip`)
- Métodos: `ExecuteAsync`, `DeleteAsync`, `GetByIdAsync`
- Registro DI: `builder.Services.AddScoped<DeleteTrip>();`

### Delete Reference (Trips)

- `DeleteTrip.ExecuteAsync(Guid id)` → verifica existência → chama `DeleteAsync` → log → retorna `bool`
- `TripRepository.DeleteAsync(Guid id)` → `FindAsync` → `Remove` → `SaveChangesAsync`
- Cascade delete já configurado: `TravelPlanConfiguration` com `OnDelete(DeleteBehavior.Cascade)` na FK `TripId`

---

## Arquitetura

A implementação segue o padrão já estabelecido no projeto:

1. **Repository** — adicionar método `DeleteAsync` no `ITravelPlanRepository` e implementar em `TravelPlanRepository`
2. **Use Case** — criar `DeleteTravelPlan` seguindo o mesmo padrão de `DeleteTrip`
3. **UI** — adicionar botões de exclusão com confirmação nas páginas existentes

---

## Estrutura Técnica

### Entidades

- `Trip` — já existe, sem alterações
- `TravelPlan` — já existe, sem alterações

### Serviços (Use Cases)

- `DeleteTrip` — já existe, sem alterações (apenas UI será adicionada)
- `DeleteTravelPlan` — novo use case

### Repositórios

- `ITripRepository` — já existe com `DeleteAsync`, sem alterações
- `ITravelPlanRepository` — adicionar método `DeleteAsync(Guid id)`
- `TravelPlanRepository` — adicionar implementação de `DeleteAsync(Guid id)`

### DTOs

- Nenhum DTO novo necessário — `DeleteTravelPlan.ExecuteAsync` recebe `Guid` diretamente, igual `DeleteTrip`

### Endpoints (Blazor Routes)

- `/trips/{Id:guid}` (TripDetails.razor) — adicionar botão "Excluir"
- `/trips/{TripId:guid}/plans/{PlanId:guid}` (PlanView.razor) — adicionar botão "Excluir"

---

## Configuração

Registrar `DeleteTravelPlan` no DI em `Program.cs`:

```csharp
builder.Services.AddScoped<DeleteTravelPlan>();
```

Nenhuma outra configuração necessária.

---

## Decisões Técnicas

- `DeleteTravelPlan` segue exatamente o padrão de `DeleteTrip`: verifica existência, loga aviso se não encontrado, loga informação se removido, retorna `bool`
- A validação de status "Processing" é feita no use case, antes da exclusão — retorna `false` se bloqueado
- O bloqueio de exclusão de planos em "Processing" usa o enum `PlanStatus` existente
- Confirmação via `confirm()` do JavaScript (simples e consistente com o escopo MVP)
- Após exclusão de viagem, navega para `/` (dashboard)
- Após exclusão de plano, navega para `/trips/{TripId}` (detalhes da viagem)
- Nenhuma nova dependência ou pacote é necessário
