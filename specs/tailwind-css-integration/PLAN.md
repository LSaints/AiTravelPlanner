# PLAN.md

## Existing Context

### Architecture

Vertical Slice / Feature-based Architecture com Blazor Interactive Server.

### Stack

- .NET 10
- ASP.NET Core (Blazor Interactive Server)
- Entity Framework Core + SQLite
- Bootstrap (estático em `wwwroot/lib/bootstrap/`)

### Existing Conventions

- CSS global em `wwwroot/app.css`
- CSS scoped em arquivos `.razor.css` por componente
- Assets estáticos servidos via `MapStaticAssets()`
- Layout raiz em `Components/App.razor`

---

## Arquitetura

O Tailwind CSS será adicionado como ferramenta de build via npm, operando em paralelo ao pipeline de assets estáticos do Blazor. O arquivo de entrada (`Styles/app.css`) conterá as diretivas `@tailwind` e será processado pelo CLI do Tailwind v4, gerando o CSS final em `wwwroot/css/app.css`. O `App.razor` referenciará o arquivo gerado como asset estático.

Não há alteração na arquitetura existente do projeto — o Tailwind atua exclusivamente na camada de apresentação.

---

## Estrutura Técnica

### Arquivos novos

- `package.json` — Configuração npm com script `dev` para watch do Tailwind
- `package-lock.json` — Gerado automaticamente pelo npm
- `Styles/app.css` — Arquivo de entrada com as diretivas `@tailwind`
- `wwwroot/css/app.css` — Arquivo de saída gerado pelo build (gitignored ou versionado)
- `.gitignore` — Atualizar (ou confirmar) que `node_modules/` está ignorado

### Arquivos modificados

- `src/ATP.Web/Components/App.razor` — Adicionar referência ao CSS compilado
- `src/ATP.Web/wwwroot/app.css` — Opcionalmente removido ou mantido (decisão: manter para compatibilidade durante migração)

---

## Configuração

### npm init

```bash
npm init -y
```

### Instalação

```bash
npm install --save-dev @tailwindcss/cli
```

### Scripts (package.json)

```json
{
  "scripts": {
    "dev": "npx @tailwindcss/cli -i ./Styles/app.css -o ./wwwroot/css/app.css --watch",
    "build": "npx @tailwindcss/cli -i ./Styles/app.css -o ./wwwroot/css/app.css"
  }
}
```

> O script `dev` usa `--watch` para desenvolvimento contínuo. O script `build` (sem `--watch`) é usado para CI/deploy.

### Styles/app.css

```css
@import "tailwindcss";
```

### App.razor

Adicionar linha:
```html
<link rel="stylesheet" href="@Assets["css/app.css"]" />
```

---

## Decisões Técnicas

- Uso do `@tailwindcss/cli` (Tailwind v4) em vez do `tailwindcss` (v3) pois o comando `npx @tailwindcss/cli` é a abordagem atual do Tailwind v4
- O arquivo de entrada fica em `Styles/app.css` (fora de `wwwroot/`) por convenção, separando fonte de build do artefato gerado
- O diretório `wwwroot/css/` será criado automaticamente pelo CLI na primeira execução
- O arquivo `wwwroot/app.css` existente será mantido para não quebrar estilos atuais durante a migração
- O comando `--watch` permite desenvolvimento contínuo com recompilação automática
- O `@Assets[...]` no `App.razor` garante que o arquivo participe do cache busting do Blazor (`MapStaticAssets`)
- O diretório `wwwroot/css/` (artefato compilado) deve ser adicionado ao `.gitignore`
- A ordem dos `<link>` no `App.razor` deve posicionar o Tailwind após Bootstrap para permitir sobrescrição de estilos, mas antes do `app.css` existente para migração gradual
