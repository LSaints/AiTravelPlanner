# SPEC.md

## Feature

Plano de Viagem Estruturado

---

## Objetivo

Substituir a resposta em texto bruto (markdown) da IA por dados estruturados (JSON), renderizados em componentes visuais ricos — cards, tabelas expansíveis, lista de dias e checklist — melhorando a legibilidade e a experiência do usuário sem alterar o banco de dados nem a interface `IAIProvider`.

---

## Requisitos de negócio

- O sistema deve instruir a IA a retornar um JSON estruturado no lugar de markdown livre
- O JSON deve seguir um schema fixo para que possa ser parseado e validado antes da exibição
- Cada seção do plano de viagem deve ser renderizada em um componente visual distinto:
  - Resumo Estratégico → card com logística e transporte
  - Itinerário Dia a Dia → cards de dia com atividades (manhã, almoço, tarde, noite)
  - Distribuição de Orçamento → tabela com categoria, descrição e valor
  - Checklist e Cuidados → lista com itens essenciais destacados
  - Notas Personalizadas → seção opcional exibida como card
- O layout deve ser responsivo e mobile-first (320px+)
- O usuário deve poder copiar o plano como texto limpo (versão textual gerada a partir dos dados estruturados)
- Planos de viagem gerados antes da mudança (markdown legado) devem continuar sendo exibidos corretamente
- O sistema deve detectar automaticamente se o conteúdo armazenado é JSON estruturado ou markdown legado

---

## Restrições

- A estrutura do banco de dados não pode ser alterada — `TravelPlan.GeneratedContent` continua como `string`
- A interface `IAIProvider` não pode ser alterada — continua retornando `Task<string>`
- O sistema de polling existente em `PlanView.razor` não pode ser modificado
- Nenhuma nova dependência externa (NuGet, npm, JS) pode ser introduzida
- O tema visual cartoon escuro existente (CSS custom properties) deve ser mantido e reutilizado
- O design deve seguir os padrões de componentes já existentes: cards com `border-width-cartoon`, `border-radius-cartoon`, `shadow-cartoon`, cores do tema
- Navegação por teclado e acessibilidade (aria-labels, landmarks) devem ser respeitadas

---

## Critérios de Aceitação

- Um plano novo gerado exibe seções separadas visualmente:
  - Card de Resumo Estratégico com logística e transporte
  - Lista de cards de dias (um por dia) com atividades detalhadas
  - Tabela de orçamento com header estilizado na cor primária
  - Lista de checklist com itens essenciais destacados (ícone ou badge)
  - Seção de notas personalizadas (se presente)
- Um plano antigo (markdown) renderiza corretamente via fallback sem quebrar o layout
- A página é funcional em 320px, 768px e 1280px — sem overflow horizontal, sem elementos sobrepostos
- Cards e tabelas seguem o estilo cartoon: borda sólida de 3px, sombra 4px 4px 0, cantos de 16px
- O botão "Copiar Plano" copia uma versão textual limpa gerada a partir dos dados estruturados
- Leitores de tela conseguem navegar entre as seções (role regions, headings)
- Nenhum teste existente quebra após as alterações
