# PLAN.md

## Existing Context

### Architecture

Modular Monolith with Clean Architecture per feature (UI / Application / Domain / Infrastructure).

### Stack

- ASP.NET Core + Blazor Server (Interactive Server)
- Entity Framework Core + SQLite
- Tailwind CSS v4 + Scoped CSS (`.razor.css`)
- Fredoka (Google Fonts)
- Dark Cartoon Theme (CSS custom properties)

### Existing Conventions

- Feature-based folders under `Features/{Feature}/UI/`
- Private nested `TripFormModel` with DataAnnotations validation in form components
- Scoped CSS files co-located with `.razor` files
- CSS custom properties for theming (`--color-primary`, `--color-bg-surface`, `--shadow-cartoon`, etc.)
- `ITripRepository` interface + `CreateTrip` use case with `CreateTripRequest` record
- Botões com classes `.btn-primary` e `.btn-secondary` padronizadas
- Animações `fadeInDown` e `fadeInUp` nos componentes

### Multi-Step Pattern

Padrão novo — não existem formulários multi-etapas no código atual. Será necessário estabelecer:
- Um mecanismo de estado de etapa (step state)
- Um componente de indicador de progresso (stepper)
- Componentes parciais para cada etapa

---

## Arquitetura

O formulário multi-etapas será implementado dentro da feature `Trips/UI/` existente, modificando `NewTrip.razor` para orquestrar as etapas e extraindo os grupos de campos para componentes Blazor separados.

O estado da etapa atual será gerenciado por uma variável `_currentStep` (int) no code-behind de `NewTrip.razor`. O `TripFormModel` existente será compartilhado entre todas as etapas via cascading parameter ou propriedade passada como parâmetro.

Uma animação de transição entre etapas (slide horizontal ou fade) será aplicada via CSS.

---

## Estrutura Técnica

### Fluxo de Navegação

```
[DESTINO] --> [ORÇAMENTO] --> [DETALHES] --> SUBMIT
   Etapa 1        Etapa 2         Etapa 3
```

### Componentes

#### StepIndicator.razor (novo)
- Local: `Features/Trips/UI/Components/`
- Renderiza indicador visual com 3 etapas
- Estados: completed, active, pending
- Design cartoon com bolinhas numeradas ou labels curtas conectadas por linha
- Mobile-first: em telas pequenas, exibir apenas círculos numerados com a label ativa destacada

#### StepDestination.razor (novo)
- Local: `Features/Trips/UI/Components/`
- Campos: Destino, País, Data de Início, Data de Término
- Recebe `TripFormModel` como parâmetro
- Layout: dois campos de data lado a lado em desktop, empilhados em mobile

#### StepBudget.razor (novo)
- Local: `Features/Trips/UI/Components/`
- Campos: Pessoas, Orçamento
- Recebe `TripFormModel` como parâmetro
- Layout: dois campos lado a lado em desktop, empilhados em mobile

#### StepDetails.razor (novo)
- Local: `Features/Trips/UI/Components/`
- Campos: O que pretende fazer, Observações adicionais
- Recebe `TripFormModel` como parâmetro
- TextAreas com altura mínima adequada

#### NewTrip.razor (modificado)
- Gerencia `_currentStep` (0, 1, 2)
- Exibe `StepIndicator` no topo
- Renderiza o componente da etapa atual condicionalmente
- Botões "Voltar" e "Próximo" / "Salvar Viagem" fora do step component
- Valida a etapa atual antes de avançar (validação manual dos campos da etapa)
- Submit final na etapa 2 (índice 2) executa `CreateTripUseCase`

### Validação por Etapa

A validação será feita manualmente checando os campos relevantes de cada etapa antes de permitir o avanço, utilizando as anotações de validação existentes no `TripFormModel` via `Validator.TryValidateObject` ou validação inline.

| Etapa | Campos validados |
|-------|------------------|
| Destino | Destination, Country, StartDate, EndDate |
| Orçamento | NumberOfPeople, Budget |
| Detalhes | Objectives |

### DTOs / Modelos

Nenhum DTO novo é necessário. O `TripFormModel` privado existente em `NewTrip.razor` será mantido e compartilhado entre os step components.

### Endpoints

Nenhum endpoint novo. A rota `/trips/new` permanece inalterada.

---

## Decisões Técnicas

- O `TripFormModel` será exposto como parâmetro de componente (`[Parameter]`) para cada step, ao invés de usar cascading parameter, para manter o fluxo de dados explícito
- `_currentStep` será um inteiro (0-2) gerenciado com métodos `GoToNextStep()` e `GoToPreviousStep()`
- A validação por etapa usará validação condicional no `HandleSubmit` ou um método `ValidateCurrentStep()` separado
- A transição entre etapas usará animação CSS fade (reaproveitando `fadeInUp` / `fadeInDown`) com trigger via mudança de classe condicional
- StepIndicator será um componente separado para reuso futuro em outros formulários multi-etapas
- Os step components não terão code-behind próprio — serão puramente de apresentação (renderização condicional via `@CurrentStep == n`)
- Navegação mobile-first: botões fixos na parte inferior, step indicator compacto com circles + label ativa

## Assumptions

- A validação dos campos de data (StartDate <= EndDate) ficará a cargo da validação no momento do submit, não por etapa
- O projeto Tailwind será compilado via `npm run build` existente, sem necessidade de novas dependências
- O tema Dark Cartoon está disponível via CSS custom properties já definidas em `Styles/app.css`
