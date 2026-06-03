# TASKS.md

## Task 1

Criar modelos de dados estruturados no domínio

### Objetivo

Criar as classes `TravelPlanData`, `StrategicSummary`, `DayItinerary`, `BudgetItem`, `ChecklistItem` e `CustomNote` em `Features/TravelPlans/Domain/` para representar o plano de viagem estruturado.

### Validação

- `TravelPlanData.cs` existe com as propriedades: `Summary`, `Days`, `Budget`, `Checklist`, `Notes`
- `StrategicSummary.cs` existe com `Logistics`, `Transportation`, `BestTimeToVisit`
- `DayItinerary.cs` existe com `Day`, `Title`, `Morning`, `Lunch`, `Afternoon`, `Evening`
- `BudgetItem.cs` existe com `Category`, `Description`, `EstimatedCost`
- `ChecklistItem.cs` existe com `Text`, `Category`, `IsEssential`
- `CustomNote.cs` existe com `Title`, `Content`
- Todas as classes seguem o namespace `ATP.Web.Features.TravelPlans.Domain`
- O projeto compila sem erros

---

## Task 2

Criar TravelPlanParser no Application layer

### Objetivo

Criar a classe estática `TravelPlanParser` em `Features/TravelPlans/Application/` com dois métodos: `TryParse` (tenta desserializar JSON em `TravelPlanData`) e `ToPlainText` (gera texto legível a partir de `TravelPlanData` para cópia).

### Validação

- `TravelPlanParser.TryParse(json)` retorna `true` e preenche `TravelPlanData` quando o JSON é válido
- `TravelPlanParser.TryParse(markdown)` retorna `false` quando o conteúdo não é JSON
- `TravelPlanParser.TryParse(jsonInvalido)` retorna `false`
- `TravelPlanParser.ToPlainText(data)` retorna um texto com todas as seções formatadas
- O texto gerado por `ToPlainText` contém resumo, dias, orçamento e checklist
- Nenhuma dependência externa nova é introduzida (usar `System.Text.Json` já disponível)

---

## Task 3

Modificar PromptBuilder para solicitar JSON estruturado

### Objetivo

Atualizar `Infrastructure/AI/PromptBuilder.cs` para instruir a IA a retornar um JSON que siga o schema de `TravelPlanData`, substituindo as instruções de formatação markdown.

### Validação

- O prompt gerado não contém mais instruções de formatação markdown (tabelas, blockquotes, etc.)
- O prompt contém a especificação do schema JSON esperado com tipos e field names
- O prompt inclui um exemplo mínimo do JSON esperado
- O prompt mantém as seções de conteúdo (Planejamento Estratégico, Itinerário, Orçamento, Checklist, Notas)
- Os parâmetros da viagem (destino, duração, viajantes, orçamento) continuam sendo passados
- Testes existentes de `PromptBuilderTests` continuam passando (ou são atualizados)

---

## Task 4

Criar componentes Blazor de renderização

### Objetivo

Criar os componentes Blazor para cada seção do plano estruturado em `Features/TravelPlans/UI/Components/`, seguindo o design cartoon escuro existente e sendo responsivos.

### Componentes a criar

1. **StrategicSummaryCard.razor + .css**
   - Card com ícone 🗺️
   - Grid de 1 coluna (mobile) / 2 colunas (≥768px)
   - Exibe logística e transporte
   - Border cartoon, sombra, cantos arredondados

2. **DayCard.razor + .css**
   - Card com header destacado (fundo primary) mostrando "Dia N — Título"
   - Seções internas: Manhã, Almoço, Tarde, Noite com labels
   - Colapsável (opcional — pode ser sempre visível)
   - Responsivo: empilha verticalmente em mobile

3. **BudgetTable.razor + .css**
   - Tabela full-width com scroll horizontal em mobile
   - Thead com background primary, linhas zebradas
   - Colunas: Categoria, Descrição, Custo Estimado
   - Linha de total no final

4. **ChecklistSection.razor + .css**
   - Lista com ícone ✔ ao lado de cada item
   - Itens essenciais com badge laranja "Essencial" e ícone ⭐
   - Grid de 1 coluna (mobile) / 2 colunas (≥768px)

5. **CustomNotesSection.razor + .css**
   - Card com ícone 📝
   - Exibe título e conteúdo

### Validação

- Cada componente aceita o respectivo modelo como `[Parameter]`
- Cada componente usa CSS scoped com as variáveis do tema (`--color-*`, `--border-*`, `--shadow-*`)
- Cada componente é responsivo (testar em 320px, 768px, 1280px)
- Os componentes seguem o padrão cartoon: borda 3px, sombra 4px 4px 0, border-radius 16px
- Nenhum componente usa Tailwind utility classes — apenas CSS scoped
- O projeto compila sem erros

---

## Task 5

Modificar PlanView.razor para usar os novos componentes com fallback

### Objetivo

Atualizar `PlanView.razor` para, no estado `Completed`, tentar parsear `GeneratedContent` como `TravelPlanData`. Se bem-sucedido, renderizar os novos componentes. Se falhar, renderizar markdown via Markdig (comportamento atual).

### Objetivo

- No estado `Completed`, o conteúdo é processado por `TravelPlanParser.TryParse`
- Se `TryParse` retorna `true`: renderiza `StrategicSummaryCard`, `DayCard` (para cada dia), `BudgetTable`, `ChecklistSection`, `CustomNotesSection` (se houver notas)
- Se `TryParse` retorna `false`: renderiza markdown via `Markdown.ToHtml` (comportamento atual)
- O botão "Copiar Plano" usa `TravelPlanParser.ToPlainText(data)` quando estruturado, ou `_plan.GeneratedContent` quando markdown
- A importação `@using Markdig` permanece condicional (ainda usada no fallback)
- O polling, loading, estados de erro e regeneração permanecem inalterados

### Validação

- Um plano com JSON válido renderiza os novos componentes corretamente
- Um plano com markdown legado renderiza via fallback (markdown) sem quebrar
- O botão "Copiar Plano" copia o texto correto em ambos os casos
- O polling continua funcionando para planos em processamento
- Navegação entre planos funciona sem erros

---

## Task 6

Atualizar CSS do PlanView.razor

### Objetivo

Simplificar o CSS de `PlanView.razor.css` removendo os estilos de renderização de conteúdo markdown (que agora ficam nos componentes) e mantendo apenas os estilos de estrutura da página (header, estados, actions).

### Validação

- Os seletores `.plan-content h1` até `.plan-content input[type="checkbox"]` são removidos (ou movidos para componentes)
- Os estilos de `.plan-header`, `.plan-meta`, `.status-badge`, `.state-message`, `.spinner`, `.failed-message`, `.plan-actions` permanecem
- A página não perde estilo visual — o layout do header e estados continua intacto
- Os novos componentes têm seus próprios estilos scoped

---

## Task 7

Atualizar testes existentes e adicionar novos

### Objetivo

Garantir que as alterações não quebram testes existentes e adicionar testes para o novo código.

### Validação

- `dotnet test` executa sem falhas
- Testes existentes de `GenerateTravelPlanTests` e `RegenerateTravelPlanTests` continuam passando
- Testes existentes de `PromptBuilderTests` continuam passando (ou são ajustados para o novo formato de prompt)
- Novos testes para `TravelPlanParser` cobrem:
  - Parse de JSON válido completo
  - Parse de JSON com campos ausentes (null handling)
  - Parse de string vazia
  - Parse de markdown (deve retornar false)
  - `ToPlainText` gera saída com todas as seções

---

## Task 8

Validar fluxo completo ponta a ponta

### Objetivo

Verificar que a feature completa funciona em todos os cenários.

### Validação

- Cenário 1 — Novo plano gerado com JSON válido:
  - A IA retorna JSON
  - O JSON é armazenado em `GeneratedContent`
  - A página renderiza os componentes corretamente
  - O botão "Copiar Plano" copia o texto limpo

- Cenário 2 — Plano legado (markdown):
  - O conteúdo markdown é detectado como não-JSON
  - O fallback renderiza o markdown corretamente
  - O botão "Copiar Plano" copia o markdown original

- Cenário 3 — Responsividade:
  - A página é utilizável em viewport de 320px (sem overflow, sem quebras)
  - A página é utilizável em 768px (tablet)
  - A página é utilizável em 1280px+ (desktop)

- Cenário 4 — Fallback de dados corrompidos:
  - Se `GeneratedContent` contém JSON inválido, o fallback markdown é ativado

- Cenário 5 — Acessibilidade:
  - Cada seção tem um heading (`h2`)
  - Navegação por tab entre botões funciona
  - Contraste de cores é suficiente (cores do tema existente)
