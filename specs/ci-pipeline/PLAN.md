# PLAN.md

## Existing Context

### Architecture

Monolito Modular (Vertical Slices) — aplicação única organizada por Features.

### Stack

- ASP.NET Core (Blazor Interactive Server)
- .NET 10 (net10.0)
- Entity Framework Core 10 + SQLite
- Tailwind CSS v4 (via @tailwindcss/cli NPM package)
- xUnit + Moq + coverlet para testes

### Existing Conventions

- Solution file: `AiTravelPlanner.slnx` (formato XML)
- Dependências .NET gerenciadas via `dotnet restore`
- Dependências NPM gerenciadas via `npm ci` / `npm install`
- Build de CSS: `npm run build` (executa Tailwind CLI)
- Testes: `dotnet test` com xUnit runner

---

## Arquitetura

Workflow único do GitHub Actions definido em `.github/workflows/ci.yml`. O pipeline será composto por stages sequenciais dentro de um único job (para simplicidade), com fallback para matriz de builds se houver necessidade futura de múltiplos SOs ou versões do SDK.

---

## Estrutura Técnica

### Workflow

- **Arquivo:** `.github/workflows/ci.yml`
- **Trigger:** `push` (branches main/master) e `pull_request` (contra main/master)
- **Runner:** `ubuntu-latest`
- **Job único:** `build-and-test`

### Etapas do Job

1. **Checkout** — `actions/checkout@v4`
2. **Setup .NET SDK** — `actions/setup-dotnet@v4` com SDK `10.0.x`
3. **Setup Node.js** — `actions/setup-node@v4` com Node LTS
4. **Restore NPM dependencies** — `npm ci` no diretório `src/ATP.Web`
5. **Build Tailwind CSS** — `npm run build` no diretório `src/ATP.Web`
6. **Restore .NET dependencies** — `dotnet restore`
7. **Build .NET** — `dotnet build --no-restore -c Release`
8. **Run tests** — `dotnet test --no-build -c Release --verbosity normal`

### Estratégia de Falha

- Cada etapa usa `Built-in fail-fast` do GitHub Actions — se qualquer etapa falhar, o job inteiro é interrompido e marcado como falha
- Nenhuma etapa possui `continue-on-error: true`

---

## Configuração

- Criar diretório `.github/workflows/` na raiz do repositório
- Adicionar arquivo `ci.yml` com a definição do workflow
- Nenhuma configuração adicional de repositório é necessária (GitHub Actions é ativado por padrão em repositórios públicos/privados)

---

## Decisões Técnicas

- **Único job vs job matrix:** Optou-se por um único job (`build-and-test`) porque o projeto não precisa ser compilado em múltiplos SOs ou versões do SDK. Caso necessário no futuro, a matrix pode ser adicionada sem quebrar o workflow existente.
- **`npm ci` em vez de `npm install`:** `npm ci` é mais rápido, usa o `package-lock.json` existente e falha se o lockfile estiver desatualizado — ideal para CI.
- **`dotnet build --no-restore`:** Evita restore duplicado já que a etapa de restore é explícita antes.
- **`dotnet test --no-build`:** Evita rebuild desnecessário pois o build já foi executado na etapa anterior.
- **Configuration `Release`:** Garante que o build de CI use a configuração Release, compatível com publicação futura.
- **Sem caching explícito:** Para um projeto pequeno com ~20 dependências NuGet e poucas dependências NPM, o tempo de restore sem cache é aceitável dentro da janela de 10 minutos. Caching pode ser adicionado como otimização futura.
- **Coverage report não incluído no MVP da pipeline:** coverlet está presente como dependência de teste, mas o upload de relatórios de cobertura (ex: Codecov) fica para uma melhoria futura.
