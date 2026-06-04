# TASKS.md

## Task 1

Adicionar método `DeleteAsync` ao repositório de TravelPlans

### Objetivo

Adicionar o método `DeleteAsync(Guid id)` na interface `ITravelPlanRepository` e implementá-lo em `TravelPlanRepository`, seguindo o mesmo padrão de `TripRepository.DeleteAsync`.

### Validação

- `ITravelPlanRepository` contém a assinatura `Task DeleteAsync(Guid id)`
- `TravelPlanRepository` implementa `DeleteAsync` usando `FindAsync` + `Remove` + `SaveChangesAsync`
- Compilação bem-sucedida

---

## Task 2

Criar use case `DeleteTravelPlan`

### Objetivo

Criar a classe `DeleteTravelPlan` em `Features/TravelPlans/Application/DeleteTravelPlan.cs`, seguindo o padrão de `DeleteTrip`:

- Construtor recebe `ITravelPlanRepository` e `ILogger<DeleteTravelPlan>`
- Método `ExecuteAsync(Guid id)` verifica existência, valida que o plano não está em "Processing", exclui e retorna `bool`
- Se o plano estiver em "Processing", loga aviso e retorna `false`
- Se o plano não existir, loga aviso e retorna `false`
- Registrar em `Program.cs` como `builder.Services.AddScoped<DeleteTravelPlan>()`

### Validação

- Classe `DeleteTravelPlan` existe em `Features/TravelPlans/Application/`
- Registro no DI está presente em `Program.cs`
- Compilação bem-sucedida

---

## Task 3

Adicionar botão de excluir viagem na UI

### Objetivo

Adicionar um botão "Excluir" na tela de detalhes da viagem (`TripDetails.razor`), ao lado do botão "Editar", com confirmação via `confirm()` JavaScript.

- Injetar `DeleteTrip` no componente
- Adicionar método `DeleteTrip()` que exibe confirmação e, se confirmado, chama `DeleteTrip.ExecuteAsync(Id)` e navega para `/`
- Se a exclusão retornar `false`, não navegar (viagem não encontrada ou cancelada)
- Usar `IJSRuntime` para `confirm()` ou `JavaScript interop`

### Validação

- Botão "Excluir" aparece no header ao lado de "Editar"
- Clicar em "Excluir" exibe diálogo de confirmação
- Confirmar remove a viagem e redireciona para o dashboard
- Cancelar mantém o usuário na tela de detalhes

---

## Task 4

Adicionar botão de excluir plano de viagem na UI

### Objetivo

Adicionar um botão "Excluir" na tela de visualização do plano (`PlanView.razor`), com confirmação e validação de status.

- Injetar `DeleteTravelPlan` no componente
- Adicionar método `DeletePlan()` com confirmação
- Se o plano estiver em "Processing", exibir mensagem de bloqueio ao invés do diálogo de confirmação
- Após exclusão bem-sucedida, navegar para `/trips/{TripId}`
- Incluir o botão no header ao lado do botão "Voltar"

### Validação

- Botão "Excluir" aparece no header do PlanView
- Clicar em "Excluir" em um plano concluído exibe confirmação
- Confirmar remove o plano e redireciona para detalhes da viagem
- Clicar em "Excluir" em um plano em "Processing" exibe mensagem de bloqueio
- Navegação correta em todos os cenários

---

## Task 5

Testar fluxo completo

### Objetivo

Validar a funcionalidade ponta a ponta, incluindo todos os cenários.

### Validação

- Cenário 1: Usuário exclui uma viagem — confirma → viagem removida → redirecionado ao dashboard
- Cenário 2: Usuário exclui uma viagem — cancela → permanece na tela de detalhes
- Cenário 3: Usuário exclui um plano de viagem concluído — confirma → plano removido → redirecionado aos detalhes da viagem
- Cenário 4: Usuário exclui um plano de viagem em "Processing" — mensagem de bloqueio exibida → exclusão não ocorre
- Cenário 5: Usuário exclui uma viagem com planos associados — cascade delete remove todos os planos
- Cenário 6: Exclusão de viagem ou plano inexistente — não causa erro, apenas redireciona
