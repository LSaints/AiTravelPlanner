# TASKS.md

## Task 1

Criar componente StepIndicator

### Objetivo

Criar o componente de indicador de progresso visual que mostra as 3 etapas (Destino, Orçamento, Detalhes) com estados completed, active e pending.

### Validação

- O componente aceita parâmetros: `TotalSteps` (int), `CurrentStep` (int), `StepLabels` (string[])
- Renderiza 3 círculos numerados ou marcadores conectados por uma linha horizontal
- O círculo da etapa atual tem destaque visual (cor primária, preenchido)
- As etapas concluídas exibem um ícone de check ou cor diferente
- As etapas futuras exibem estilo atenuado (opacity reduzida, borda tracejada)
- Em mobile (< 640px), exibe apenas os círculos com a label da etapa ativa abaixo
- Em desktop, exibe círculos + labels curtas lado a lado
- Animações de transição quando o step muda

---

## Task 2

Criar StepDestination component

### Objetivo

Extrair os campos de destino (Destino, País, Data de Início, Data de Término) do `NewTrip.razor` para um componente `StepDestination.razor` independente.

### Validação

- Componente aceita `TripFormModel` como parâmetro
- Renderiza os campos: Destino (InputText), País (InputText), Data de Início (InputDate), Data de Término (InputDate)
- Inclui `ValidationMessage` para cada campo
- Layout: datas lado a lado em desktop (> 640px), empilhadas em mobile
- Estilos consistentes com as classes `.form-group`, `.form-input` existentes
- Não possui lógica de navegação — é puramente de apresentação

---

## Task 3

Criar StepBudget component

### Objetivo

Extrair os campos de orçamento (Pessoas, Orçamento) do `NewTrip.razor` para um componente `StepBudget.razor` independente.

### Validação

- Componente aceita `TripFormModel` como parâmetro
- Renderiza os campos: Pessoas (InputNumber), Orçamento (InputNumber)
- Inclui `ValidationMessage` para cada campo
- Layout: campos lado a lado em desktop (> 640px), empilhados em mobile
- Estilos consistentes com as classes existentes
- Não possui lógica de navegação

---

## Task 4

Criar StepDetails component

### Objetivo

Extrair os campos de detalhes (O que pretende fazer, Observações adicionais) do `NewTrip.razor` para um componente `StepDetails.razor` independente.

### Validação

- Componente aceita `TripFormModel` como parâmetro
- Renderiza os campos: Objectives (InputTextArea), AdditionalNotes (InputTextArea)
- Inclui `ValidationMessage` para cada campo
- TextAreas com altura mínima adequada (min-height: 80px)
- Estilos consistentes com as classes existentes
- Não possui lógica de navegação

---

## Task 5

Modificar NewTrip.razor para orquestração multi-etapas

### Objetivo

Refatorar `NewTrip.razor` para gerenciar o estado de etapa (`_currentStep`), renderizar o `StepIndicator` e o componente da etapa atual, e implementar navegação entre etapas com validação por etapa.

### Validação

- `_currentStep` inicia em 0 (Destino)
- `StepIndicator` é exibido no topo do formulário com `CurrentStep` sincronizado
- A etapa atual renderiza o componente correspondente (`StepDestination`, `StepBudget`, ou `StepDetails`)
- Botão "Próximo" avança para a próxima etapa se a validação da etapa atual passar
- Botão "Voltar" retorna à etapa anterior (não exibido na primeira etapa)
- Na última etapa, o botão exibe "Salvar Viagem" e executa o submit
- Validação por etapa: campos obrigatórios da etapa atual são validados antes de avançar
- Método `ValidateCurrentStep()` implementa a validação manual usando `Validator.TryValidateObject` ou verificação inline dos campos da etapa
- Estado de loading (`_saving`) é exibido apenas no submit final
- Cancelamento redireciona para `/` conforme comportamento atual
- Transição animada (fade) entre etapas usando classes CSS condicionais

---

## Task 6

Estilizar StepIndicator e transições entre etapas

### Objetivo

Criar o arquivo `StepIndicator.razor.css` com estilos cartoon para o stepper e adicionar animações de transição entre etapas no `NewTrip.razor.css`.

### Validação

- StepIndicator segue o Dark Cartoon Theme (bordas 3px, sombra cartoon, fonte Fredoka, cor primária laranja)
- Círculos numerados com tamanho adequado (40px em desktop, 32px em mobile)
- Linha conectora entre círculos com cor de borda consistente
- Etapa ativa: fundo laranja (`--color-primary`), texto escuro (`--color-text-on-primary`)
- Etapa concluída: fundo verde suave ou laranja com opacidade, ícone de check
- Etapa pendente: borda apenas, sem preenchimento
- Transição entre etapas: animação fadeInUp/fadeInDown de 0.3s-0.4s
- Em mobile (< 640px): stepper compacto, circles menores, apenas label ativa visível
- Botões "Voltar" e "Próximo/Salvar" em posição fixa na parte inferior ou logo abaixo do formulário

---

## Task 7

Testar fluxo completo do formulário multi-etapas

### Objetivo

Validar o comportamento completo do formulário multi-etapas, incluindo navegação, validação e submit.

### Validação

- Cenário 1: Usuário preenche etapa Destino corretamente e avança → etapa Orçamento é exibida
- Cenário 2: Usuário tenta avançar sem preencher campos obrigatórios → validação impede avanço e exibe mensagens
- Cenário 3: Usuário navega entre etapas (Voltar/Próximo) → dados preenchidos são preservados
- Cenário 4: Usuário completa todas as etapas e salva → `CreateTripUseCase.ExecuteAsync` é chamado com dados corretos
- Cenário 5: Usuário clica Cancelar → redireciona para `/`
- Cenário 6: Em viewport mobile (< 640px) → todos os campos e botões ocupam 100% da largura
- Cenário 7: Indicador de progresso reflete corretamente o estado (completed/active/pending) em cada etapa
- Cenário 8: Transição animada entre etapas ocorre suavemente sem quebras visuais
