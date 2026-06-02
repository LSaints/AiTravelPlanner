# TASKS.md

## Task 1

Definir variáveis CSS do tema escuro cartoon no arquivo de entrada do Tailwind

### Objetivo

Adicionar as variáveis CSS da paleta de cores escuras e do estilo cartoon (bordas arredondadas, sombras, bordas grossas) no arquivo `Styles/app.css` dentro de um `@layer theme` do Tailwind v4.

### Validação

- O arquivo `Styles/app.css` contém as variáveis `--color-primary`, `--color-bg-base`, `--color-text-primary`, `--border-radius-cartoon`, `--shadow-cartoon`, entre outras definidas no PLAN.md
- As variáveis estão encapsuladas em `@layer theme`
- `npm run build` compila sem erros
- As variáveis estão acessíveis no inspetor de estilos do navegador no elemento `:root`

---

## Task 2

Aplicar tema escuro cartoon no MainLayout

### Objetivo

Ajustar `MainLayout.razor` e `MainLayout.razor.css` para usar o fundo escuro (`--color-bg-base`), remover estilos fixos claros e aplicar o estilo cartoon nos elementos estruturais (sidebar, top-row, área de conteúdo).

### Validação

- O fundo da página utiliza a cor `--color-bg-base`
- A sidebar utiliza `--color-bg-sidebar` como fundo
- O top-row utiliza `--color-bg-surface`
- As bordas e sombras seguem os tokens `--border-radius-cartoon` e `--shadow-cartoon`
- O layout permanece funcional em viewports mobile e desktop

---

## Task 3

Aplicar tema escuro cartoon no NavMenu

### Objetivo

Ajustar `NavMenu.razor` e `NavMenu.razor.css` para usar o tema escuro, com destaque em laranja nos links ativos, cantos arredondados nos itens de navegação e bordas cartoon.

### Validação

- O fundo do NavMenu usa `--color-bg-sidebar`
- O link ativo tem fundo ou borda em `--color-primary` (laranja)
- Os itens de navegação têm `--border-radius-cartoon` e `--border-width-cartoon`
- O hover dos itens usa `--color-primary-hover` ou variação
- O texto dos links usa `--color-text-primary` e `--color-text-secondary`

---

## Task 4

Aplicar tema escuro cartoon na página Home

### Objetivo

Ajustar `Components/Pages/Home.razor` para usar as cores e estilos do novo tema, com fundo escuro, texto claro e títulos em laranja com peso bold.

### Validação

- O fundo da página usa `--color-bg-base`
- O título "Home" está em laranja (`--color-primary`) com `font-bold`
- O texto de boas-vindas usa `--color-text-primary`
- A página mantém o layout responsivo

---

## Task 5

Aplicar tema escuro cartoon nos componentes de TravelPlans

### Objetivo

Ajustar `Features/TravelPlans/UI/PlanView.razor` e seu CSS scoped para utilizar as variáveis do tema escuro cartoon, com fundo escuro, cards com cantos arredondados e destaques em laranja.

### Validação

- O fundo do componente usa `--color-bg-base` ou `--color-bg-surface`
- Cards ou containers usam `--border-radius-cartoon`, `--border-width-cartoon` e `--shadow-cartoon`
- Bordas dos cards usam `--color-border`
- Texto segue `--color-text-primary` e `--color-text-secondary`
- Elementos interativos (botões, links) usam `--color-primary`

---

## Task 6

Aplicar tema escuro cartoon nos componentes de Trips

### Objetivo

Ajustar os CSS scoped de `Features/Trips/UI/` (Dashboard, EditTrip, NewTrip, TripDetails) para utilizar as variáveis do tema escuro cartoon, garantindo consistência visual em toda a funcionalidade de viagens.

### Validação

- Dashboard.razor.css usa as variáveis do tema para fundo, cards e destaques
- EditTrip.razor.css e NewTrip.razor.css usam o tema em formulários (inputs com bordas cartoon, botões laranja)
- TripDetails.razor.css exibe detalhes com fundo escuro e destaques em laranja
- Todos os botões primários têm fundo `--color-primary` com texto `--color-text-on-primary`
- Inputs e campos de formulário têm `--border-radius-cartoon` e `--border-width-cartoon`

---

## Task 7

Ajustar estilos globais residuais no wwwroot/app.css (opcional)

### Objetivo

Revisar o arquivo `wwwroot/app.css` (CSS original) e remover ou sobrescrever regras que conflitem com o tema escuro cartoon, garantindo que não haja estilos claros residuais visíveis.

### Validação

- Nenhum estilo claro do `wwwroot/app.css` sobrepõe o fundo escuro do tema
- Se houver conflito, a regra é ajustada ou removida
- A aplicação não exibe flashes de fundo claro durante o carregamento

---

## Task 8

Testar fluxo completo do tema escuro cartoon

### Objetivo

Validar o tema ponta a ponta em todos os componentes e páginas da aplicação.

### Validação

- A página Home renderiza com fundo escuro, título laranja bold e texto claro
- O menu de navegação lateral exibe links com destaque laranja no ativo e hover cartoon
- Os cards de TravelPlans têm cantos arredondados, bordas grossas e sombra com deslocamento
- Os formulários de Trips (criação, edição, detalhes) seguem o estilo cartoon com inputs arredondados e botões laranja
- O Dashboard de Trips exibe dados com fundo escuro e destaques consistentes
- A aplicação é responsiva e o tema se mantém em mobile e desktop
- Não há regressão visual — componentes existentes continuam funcionais
- O build do Tailwind (`npm run build`) compila sem erros
