# PLAN.md

## Existing Context

### Architecture

Vertical Slice / Feature-based Architecture com Blazor Interactive Server (.NET 10).

### Stack

- .NET 10
- ASP.NET Core (Blazor Interactive Server)
- Entity Framework Core + SQLite
- Tailwind CSS v4 (via `@tailwindcss/cli`)
- CSS scoped (`.razor.css`)

### Existing Conventions

- Features organizadas em `Features/<FeatureName>/` com subpastas `Application/`, `Domain/`, `UI/`
- CSS global em `Styles/app.css` (entrada Tailwind)
- CSS scoped por componente em arquivos `.razor.css`
- Layout raiz em `Components/App.razor`
- Sidebar de navegação em `Components/Layout/NavMenu.razor`
- Páginas em `Components/Pages/`
- Classes utilitárias Tailwind para estilização inline
- Assets estáticos servidos via `MapStaticAssets()`

---

## Arquitetura

O tema será implementado através de variáveis CSS customizadas definidas globalmente no arquivo de entrada do Tailwind (`Styles/app.css`). A paleta de cores escuras e o estilo cartoon (bordas arredondadas, sombras, bordas grossas) serão aplicados via configuração centralizada, permitindo que todos os componentes existentes consumam as variáveis do tema sem necessidade de alterações manuais em cada arquivo.

Os componentes de layout (MainLayout, NavMenu) e os componentes de UI dos features existentes (TravelPlans, Trips) serão ajustados pontualmente para substituir estilos fixos pelo uso das variáveis do tema.

Não há criação de novos frameworks ou bibliotecas — o Tailwind CSS v4 com CSS custom properties é suficiente.

---

## Estrutura Técnica

### Variáveis CSS (Tema)

Definidas em `Styles/app.css`:

- `--color-primary`: Laranja (#ff6b35)
- `--color-primary-hover`: Laranja mais escuro (#e55a2b)
- `--color-primary-light`: Laranja claro (#ff8c5a)
- `--color-bg-base`: Fundo escuro principal (#1a1a2e)
- `--color-bg-surface`: Superfície de cards/painéis (#16213e)
- `--color-bg-elevated`: Elementos elevados (modais, dropdowns) (#0f3460)
- `--color-bg-sidebar`: Fundo da barra lateral (#0f0f23)
- `--color-text-primary`: Texto primário (#f0f0f0)
- `--color-text-secondary`: Texto secundário (#b0b0c0)
- `--color-text-on-primary`: Texto sobre fundo laranja (#1a1a2e)
- `--color-border`: Cor das bordas (#ff6b35 com 40% opacidade)
- `--border-radius-cartoon`: Raio de borda cartoon (16px)
- `--border-radius-sm`: Raio pequeno (8px)
- `--border-width-cartoon`: Largura da borda cartoon (3px)
- `--shadow-cartoon`: Sombra com deslocamento (4px 4px 0px rgba(0,0,0,0.3))
- `--font-weight-heading`: Peso da fonte para títulos (700)

### Arquivos modificados

- `Styles/app.css` — Adicionar variáveis CSS do tema escuro cartoon na diretiva `@layer theme`
- `Components/Layout/MainLayout.razor` — Aplicar classes de fundo escuro e remover estilos fixos claros
- `Components/Layout/MainLayout.razor.css` — Substituir cores fixas por variáveis do tema
- `Components/Layout/NavMenu.razor` — Aplicar classes do tema cartoon
- `Components/Layout/NavMenu.razor.css` — Substituir cores fixas por variáveis, aplicar cantos arredondados e bordas
- `Components/Pages/Home.razor` — Ajustar para usar classes do tema
- `Features/TravelPlans/UI/PlanView.razor` — Ajustar classes para o tema escuro cartoon
- `Features/TravelPlans/UI/PlanView.razor.css` — Substituir cores fixas por variáveis
- `Features/Trips/UI/Dashboard.razor` — Ajustar classes
- `Features/Trips/UI/Dashboard.razor.css` — Substituir cores fixas por variáveis
- `Features/Trips/UI/EditTrip.razor` — Ajustar classes
- `Features/Trips/UI/EditTrip.razor.css` — Substituir cores fixas por variáveis
- `Features/Trips/UI/NewTrip.razor` — Ajustar classes
- `Features/Trips/UI/NewTrip.razor.css` — Substituir cores fixas por variáveis
- `Features/Trips/UI/TripDetails.razor` — Ajustar classes
- `Features/Trips/UI/TripDetails.razor.css` — Substituir cores fixas por variáveis

---

## Configuração

### Paleta de Cores

| Token | Cor | Hex |
|-------|-----|-----|
| Primary | Laranja cartoon | `#ff6b35` |
| Primary Hover | Laranja escuro | `#e55a2b` |
| Primary Light | Laranja claro | `#ff8c5a` |
| Base Background | Azul marinho escuro | `#1a1a2e` |
| Surface | Azul escuro | `#16213e` |
| Elevated | Azul médio-escuro | `#0f3460` |
| Sidebar | Preto azulado | `#0f0f23` |
| Text Primary | Branco suave | `#f0f0f0` |
| Text Secondary | Cinza claro | `#b0b0c0` |
| Text on Primary | Preto | `#1a1a2e` |

### Estilo Cartoon

| Propriedade | Valor |
|-------------|-------|
| border-radius (padrão) | 16px |
| border-radius (pequeno) | 8px |
| border-width | 3px |
| box-shadow | `4px 4px 0px rgba(0,0,0,0.3)` |
| font-weight (headings) | 700 (bold) |

---

## Decisões Técnicas

- As variáveis CSS serão definidas no próprio `Styles/app.css` dentro de um `@layer theme` do Tailwind para manter a integração com o sistema de layers do Tailwind v4
- O tema é global e aplicado por padrão — não haverá seletor `[data-theme]` ou toggle, pois o requisito é tema escuro fixo
- O estilo cartoon será aplicado via variáveis CSS e classes utilitárias Tailwind, sem necessidade de framework CSS adicional
- Componentes existentes em `Features/` terão seus CSS scoped ajustados para referenciar as variáveis do tema, garantindo consistência
- O arquivo `wwwroot/app.css` existente (CSS puro original) será mantido sem alterações para não quebrar estilos residuais, mas será suplantado pelas variáveis do tema nos componentes ativos
- Tipografia cartoon será obtida com `font-bold` do Tailwind + tamanhos generosos (text-lg, text-xl) — sem necessidade de font-face customizada
- O contraste de acessibilidade é mantido usando laranja apenas em fundos escuros com texto escuro sobre laranja, e texto claro sobre fundo escuro
