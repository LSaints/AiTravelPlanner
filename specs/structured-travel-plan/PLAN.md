# PLAN.md

## Existing Context

### Architecture

Feature-based Clean Architecture (Monolito Modular)

```
Features/{Feature}/
├── Domain/       → Entities, enums
├── Application/  → Use cases, repository interfaces, services
└── UI/           → Blazor pages, scoped CSS
```

### Stack

- C# .NET 10
- ASP.NET Core with Blazor Server (Interactive Server)
- Entity Framework Core 10.0.8 (SQLite)
- Tailwind CSS v4 (via CLI)
- xUnit + Moq para testes

### Existing Conventions

- Use cases como classes scoped com método `ExecuteAsync`
- Repository interfaces em `Application/`, implementações em `Infrastructure/Persistence/Repositories/`
- Namespace pattern: `ATP.Web.Features.{Feature}.{Layer}`
- CSS scoped por componente (`.razor.css`)
- Tema cartoon escuro via CSS custom properties em `Styles/app.css`
- Prompt gerado por `PromptBuilder.Build(Trip)` (static)
- `IAIProvider` retorna `Task<string>` — não deve ser alterado

### Existing Folder Structure

```
Features/TravelPlans/
├── Domain/
│   ├── TravelPlan.cs
│   └── PlanStatus.cs
├── Application/
│   ├── ITravelPlanRepository.cs
│   ├── GenerateTravelPlan.cs
│   └── RegenerateTravelPlan.cs
└── UI/
    ├── PlanView.razor
    └── PlanView.razor.css
```

---

## Arquitetura

A interface `IAIProvider` permanece inalterada (retorna `string`). O prompt é modificado para solicitar JSON. O JSON bruto é armazenado em `TravelPlan.GeneratedContent`. Na renderização, o conteúdo é parseado como `TravelPlanData`; se o parsing falhar, o conteúdo é tratado como markdown legado e renderizado via Markdig (comportamento atual).

A detecção entre JSON e markdown é feita pelo `TravelPlanParser`: se o conteúdo inicia com `{` e é um JSON válido, os dados estruturados são usados. Caso contrário, o fallback de markdown é ativado.

```
PromptBuilder (modificado)
  → IA retorna JSON
  → Armazenado em TravelPlan.GeneratedContent (string)
  → TravelPlanParser tenta parsear como TravelPlanData
    ├── Sucesso → renderiza componentes (cards, tabela, etc.)
    └── Falha   → renderiza markdown via Markdig (fallback)
```

---

## Estrutura Técnica

### Entidades (Domain)

```
TravelPlans/Domain/
├── TravelPlan.cs         (existente — sem alterações)
├── PlanStatus.cs         (existente — sem alterações)
├── TravelPlanData.cs     (novo — modelo estruturado)
├── StrategicSummary.cs   (novo)
├── DayItinerary.cs       (novo)
├── BudgetItem.cs         (novo)
├── ChecklistItem.cs      (novo)
└── CustomNote.cs         (novo)
```

`TravelPlanData` — raiz do JSON retornado pela IA:
```csharp
public class TravelPlanData
{
    public StrategicSummary Summary { get; set; }
    public List<DayItinerary> Days { get; set; }
    public List<BudgetItem> Budget { get; set; }
    public List<ChecklistItem> Checklist { get; set; }
    public List<CustomNote>? Notes { get; set; }
}
```

`StrategicSummary` — logística, transporte, época ideal.

`DayItinerary` — dia, título, manhã, almoço, tarde, noite.

`BudgetItem` — categoria, descrição, custo estimado.

`ChecklistItem` — texto, categoria, essencial (bool).

`CustomNote` — título, conteúdo.

### Serviços (Application)

```
TravelPlans/Application/
├── ITravelPlanRepository.cs     (existente — sem alterações)
├── GenerateTravelPlan.cs        (existente — sem alterações)
├── RegenerateTravelPlan.cs      (existente — sem alterações)
└── TravelPlanParser.cs          (novo)
```

`TravelPlanParser` — serviço estático com dois métodos:
- `TryParse(string content, out TravelPlanData? data)` — tenta parsear JSON; retorna `bool`
- `ToPlainText(TravelPlanData data)` — gera texto legível para cópia

### PromptBuilder (modificado)

`Infrastructure/AI/PromptBuilder.cs` — o método `Build` é atualizado para instruir a IA a retornar um JSON no lugar de markdown. O JSON deve seguir o schema definido pelas entidades acima. A instrução de formatação markdown é substituída por uma especificação do schema JSON esperado, com exemplos de valores.

### UI — Novos Componentes

```
TravelPlans/UI/
├── PlanView.razor              (modificado)
├── PlanView.razor.css          (modificado — simplificado, mantém apenas states/fallback)
└── Components/
    ├── StrategicSummaryCard.razor
    ├── StrategicSummaryCard.razor.css
    ├── DayCard.razor
    ├── DayCard.razor.css
    ├── BudgetTable.razor
    ├── BudgetTable.razor.css
    ├── ChecklistSection.razor
    ├── ChecklistSection.razor.css
    ├── CustomNotesSection.razor
    └── CustomNotesSection.razor.css
```

Cada componente:
- Recebe o respectivo modelo como parâmetro
- Usa CSS scoped com as variáveis do tema (`--color-primary`, `--color-bg-surface`, `--border-width-cartoon`, etc.)
- Segue o padrão cartoon: `border: var(--border-width-cartoon) solid var(--color-border)`, `border-radius: var(--border-radius-cartoon)`, `box-shadow: var(--shadow-cartoon)`
- É responsivo (flexbox/grid, sem widths fixas em px)

#### StrategicSummaryCard.razor
- Card com ícone e informações de logística + transporte
- Grid de 1 coluna (mobile) / 2 colunas (desktop)

#### DayCard.razor
- Card colapsável com número do dia no header (fundo primary)
- Seções internas: manhã, almoço, tarde, noite
- Cada seção com label e texto

#### BudgetTable.razor
- Tabela com `thead` estilizado e linhas zebradas
- Colunas: Categoria, Descrição, Custo Estimado
- Total calculado no rodapé

#### ChecklistSection.razor
- Lista de itens com ícone de check
- Itens essenciais com badge "Essencial"

#### CustomNotesSection.razor
- Card simples com conteúdo

### UI — PlanView.razor (modificado)

- Lógica de loading, polling, status e regeneração permanece igual
- No estado `Completed`, substitui a renderização markdown por:
  - Tentar parsear `_plan.GeneratedContent` como `TravelPlanData`
  - Se sucesso: renderiza os componentes em sequência
  - Se falha: renderiza markdown via Markdig (fallback)
- O botão "Copiar Plano" copia a saída de `TravelPlanParser.ToPlainText(data)` quando estruturado, ou `_plan.GeneratedContent` quando markdown

### Endpoints

- Nenhuma alteração — rotas Blazor existentes permanecem

---

## Configuração

- Nenhuma nova configuração necessária
- Nenhuma migration de banco de dados
- Nenhuma nova variável de ambiente

---

## Decisões Técnicas

- **Manter `IAIProvider` inalterado**: a abstração permanece genérica (`Task<string>`) para não acoplar o provedor de IA ao domínio de planos de viagem. O parsing do JSON é responsabilidade da camada de aplicação (`TravelPlanParser`).
- **Armazenar JSON bruto em `GeneratedContent`**: evita migration de banco e permite fallback para planos legados. O schema dos dados é auto-descritivo e versionado pelas entidades.
- **Fallback automático markdown → estruturado**: a detecção por parsing elimina a necessidade de uma flag/coluna extra no banco. Qualquer conteúdo que não seja JSON válido é tratado como markdown.
- **Componentes Blazor separados por seção**: facilita manutenção, teste e reuso. Cada componente tem responsabilidade única e CSS scoped.
- **PromptBuilder modificado para JSON**: a IA recebe o schema JSON esperado no prompt, com instruções claras de tipo e formato. Isso garante que o retorno seja parseável.
- **Sem Tailwind utility classes nos novos componentes**: os componentes seguem o padrão CSS scoped com variáveis do tema (como os demais componentes do projeto), mantendo consistência visual.

### Assumptions

- A IA (Gemini) é capaz de retornar JSON válido seguindo o schema especificado no prompt
- O JSON retornado pela IA cabe dentro do limite de caracteres da coluna `GeneratedContent`
- Nenhum plano existente no banco contém JSON que coincidentemente seja parseável como `TravelPlanData` (risco baixo — planos atuais são markdown com cabeçalhos, não JSON)
