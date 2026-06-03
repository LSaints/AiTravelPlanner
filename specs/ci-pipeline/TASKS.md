# TASKS.md

## Task 1

Criar estrutura do workflow e etapa de checkout

### Objetivo

Criar o diretório `.github/workflows/` e o arquivo `ci.yml` com o trigger configurado para push e pull_request, contendo a etapa inicial de checkout.

### Validação

- Arquivo `.github/workflows/ci.yml` existe no repositório
- Workflow aparece na aba "Actions" do GitHub
- Trigger está configurado para `push` e `pull_request`

---

## Task 2

Configurar SDK do .NET e Node.js

### Objetivo

Adicionar etapas de setup do .NET 10 SDK e Node.js LTS ao workflow, garantindo que as ferramentas corretas estejam disponíveis para restore, build e testes.

### Validação

- Workflow executa `actions/setup-dotnet@v4` com sucesso
- Workflow executa `actions/setup-node@v4` com sucesso
- Versão do .NET e Node são exibidas no log da etapa

---

## Task 3

Restaurar dependências NPM e compilar assets Tailwind

### Objetivo

Adicionar etapas de `npm ci` e `npm run build` no diretório `src/ATP.Web`, garantindo que os assets CSS sejam gerados antes do build .NET.

### Validação

- `npm ci` executa sem erros
- `npm run build` gera o arquivo `wwwroot/css/app.css`
- Workflow falha se alguma dependência NPM estiver ausente ou se o build do Tailwind falhar

---

## Task 4

Restaurar dependências .NET e executar build

### Objetivo

Adicionar etapas de `dotnet restore` e `dotnet build -c Release` ao workflow, garantindo que o projeto compile corretamente.

### Validação

- `dotnet restore` executa sem erros
- `dotnet build -c Release` conclui com sucesso (exit code 0)
- Workflow falha se houver erro de compilação

---

## Task 5

Executar testes automatizados

### Objetivo

Adicionar etapa de `dotnet test -c Release --no-build` ao workflow, garantindo que todos os testes unitários passem.

### Validação

- `dotnet test` executa todos os testes do projeto `ATP.Web.Tests`
- Workflow falha se algum teste falhar
- Saída do teste exibe total de testes executados, passados e falhados
- Status check aparece no PR e no commit

---

## Task 6

Testar fluxo completo da pipeline

### Objetivo

Validar a pipeline ponta a ponta abrindo um Pull Request de teste contra a branch main.

### Validação

- Push para branch de feature dispara a pipeline automaticamente
- Abertura de PR contra main dispara a pipeline automaticamente
- Todas as etapas (checkout, setup, restore, build assets, build .NET, testes) executam em sequência
- Pipeline conclui com status "success" em menos de 10 minutos
- Status check aparece no PR como "passing"
- Pipeline falha corretamente quando um teste é intencionalmente quebrado
