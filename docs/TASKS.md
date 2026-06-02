# TASKS.md

## Task 1

Criar entidade `Trip` no módulo `Trips/Domain`

### Objetivo

Representar uma viagem criada pelo usuário.

### Validação

* Arquivo `Features/Trips/Domain/Trip.cs` existe
* Classe `Trip` possui:

    * Id
    * Destination
    * Country
    * StartDate
    * EndDate
    * NumberOfPeople
    * Budget
    * Objectives
    * AdditionalNotes
    * CreatedAt
* Id é do tipo Guid
* Budget é decimal
* Datas utilizam DateOnly
* Classe compila sem erros

---

## Task 2

Criar enum `PlanStatus`

### Objetivo

Representar o estado da geração de um plano de viagem.

### Validação

* Arquivo `Features/TravelPlans/Domain/PlanStatus.cs` existe
* Enum possui:

    * Pending
    * Processing
    * Completed
    * Failed

---

## Task 3

Criar entidade `TravelPlan`

### Objetivo

Persistir os planos gerados pela IA.

### Validação

* Arquivo `Features/TravelPlans/Domain/TravelPlan.cs` existe
* Classe possui:

    * Id
    * TripId
    * Prompt
    * GeneratedContent
    * PromptVersion
    * AiProvider
    * Model
    * TokensUsed
    * EstimatedCost
    * Status
    * GeneratedAt
* Status utiliza PlanStatus
* TripId é Guid

---

## Task 4

Criar interface `ITripRepository`

### Objetivo

Abstrair a persistência das viagens.

### Validação

* Arquivo `Features/Trips/Application/ITripRepository.cs` existe
* Interface possui:

    * GetByIdAsync
    * GetAllAsync
    * CreateAsync
    * UpdateAsync
    * DeleteAsync

---

## Task 5

Criar interface `ITravelPlanRepository`

### Objetivo

Abstrair a persistência dos planos.

### Validação

* Arquivo `Features/TravelPlans/Application/ITravelPlanRepository.cs` existe
* Interface possui:

    * GetByIdAsync
    * GetByTripAsync
    * CreateAsync

---

## Task 6

Criar contrato `IAIProvider`

### Objetivo

Desacoplar a aplicação de provedores específicos de IA.

### Validação

* Arquivo `Shared/Abstractions/IAIProvider.cs` existe
* Interface possui:

    * GenerateAsync(string prompt, CancellationToken cancellationToken)
* Retorna Task<string>

---

## Task 7

Criar classe `ApplicationDbContext`

### Objetivo

Configurar o Entity Framework Core.

### Validação

* Arquivo `Infrastructure/Persistence/ApplicationDbContext.cs` existe
* Possui DbSet<Trip>
* Possui DbSet<TravelPlan>
* Herda de DbContext

---

## Task 8

Criar configuração EF Core para `Trip`

### Objetivo

Mapear a entidade Trip para SQLite.

### Validação

* Existe configuração para Trip
* Tabela chama-se Trips
* Id é chave primária
* Campos obrigatórios configurados

---

## Task 9

Criar configuração EF Core para `TravelPlan`

### Objetivo

Mapear a entidade TravelPlan para SQLite.

### Validação

* Existe configuração para TravelPlan
* Tabela chama-se TravelPlans
* Relacionamento com Trip configurado
* Migration é criada sem erros

---

## Task 10

Implementar `TripRepository`

### Objetivo

Persistir viagens utilizando EF Core.

### Validação

* Implementa ITripRepository
* Todos os métodos funcionam
* Registrado no Dependency Injection

---

## Task 11

Implementar `TravelPlanRepository`

### Objetivo

Persistir planos utilizando EF Core.

### Validação

* Implementa ITravelPlanRepository
* Todos os métodos funcionam
* Registrado no Dependency Injection

---

## Task 12

Criar Use Case `CreateTrip`

### Objetivo

Permitir criação de viagens.

### Validação

* Recebe dados da viagem
* Cria entidade Trip
* Salva utilizando ITripRepository
* Retorna viagem criada

---

## Task 13

Criar Use Case `UpdateTrip`

### Objetivo

Permitir edição de viagens.

### Validação

* Busca viagem existente
* Atualiza dados
* Persiste alterações

---

## Task 14

Criar Use Case `DeleteTrip`

### Objetivo

Permitir exclusão de viagens.

### Validação

* Busca viagem
* Remove registro
* Retorna sucesso

---

## Task 15

Criar Use Case `GetTrip`

### Objetivo

Consultar uma viagem específica.

### Validação

* Busca por Id
* Retorna Trip
* Retorna nulo quando não encontrada

---

## Task 16

Criar classe `PromptBuilder`

### Objetivo

Centralizar a construção dos prompts enviados para IA.

### Validação

* Arquivo `Infrastructure/AI/PromptBuilder.cs` existe
* Recebe Trip
* Gera prompt estruturado
* Inclui destino
* Inclui orçamento
* Inclui objetivos
* Inclui período da viagem

---

## Task 17

Criar `GeminiProvider`

### Objetivo

Integrar a aplicação ao Gemini.

### Validação

* Implementa IAIProvider
* Consome Gemini API
* Retorna resposta textual
* Configuração vem de Options Pattern

---

## Task 18

Criar Use Case `GenerateTravelPlan`

### Objetivo

Gerar um plano de viagem usando IA.

### Validação

* Busca Trip
* Cria TravelPlan com Status Pending
* Gera prompt
* Chama IAIProvider
* Atualiza conteúdo gerado
* Atualiza Status para Completed
* Em caso de erro atualiza Status para Failed

---

## Task 19

Criar Use Case `RegenerateTravelPlan`

### Objetivo

Permitir nova geração para a mesma viagem.

### Validação

* Não sobrescreve plano anterior
* Cria novo TravelPlan
* Mantém histórico completo

---

## Task 20

Criar página Dashboard

### Objetivo

Listar viagens existentes.

### Validação

* Página existe
* Lista viagens
* Possui navegação para detalhes
* Funciona em dispositivos móveis

---

## Task 21

Criar página Nova Viagem

### Objetivo

Permitir cadastro de viagens.

### Validação

* Formulário criado
* Todos os campos de Trip disponíveis
* Validação visual funcionando
* Salva viagem com sucesso

---

## Task 22

Criar página Editar Viagem

### Objetivo

Permitir alteração de viagens.

### Validação

* Carrega viagem existente
* Atualiza dados corretamente

---

## Task 23

Criar página Detalhes da Viagem

### Objetivo

Visualizar informações da viagem.

### Validação

* Exibe dados da viagem
* Exibe histórico de TravelPlans
* Possui botão Gerar Plano

---

## Task 24

Criar página Visualização do Plano

### Objetivo

Exibir o plano gerado pela IA.

### Validação

* Renderiza Markdown
* Exibe status
* Exibe data da geração
* Exibe modelo utilizado

---

## Task 25

Configurar Logging

### Objetivo

Registrar eventos importantes da aplicação.

### Validação

* Log de criação de viagem
* Log de atualização
* Log de exclusão
* Log de geração de plano
* Log de falhas Gemini

---

## Task 26

Configurar Segurança Básica

### Objetivo

Proteger o MVP contra uso indevido.

### Validação

* HTTPS habilitado
* Rate Limiting configurado
* API Key não está hardcoded
* Inputs são validados

---

## Task 27

Criar container Docker

### Objetivo

Permitir deploy simplificado.

### Validação

* Dockerfile existe
* Aplicação executa em container
* SQLite persiste dados corretamente

---

## Task 28

Executar validação completa do MVP

### Objetivo

Garantir que todos os fluxos principais funcionam.

### Validação

* Criar viagem
* Editar viagem
* Excluir viagem
* Gerar plano
* Regenerar plano
* Consultar histórico
* Reiniciar aplicação sem perder dados
* Executar aplicação via Docker
