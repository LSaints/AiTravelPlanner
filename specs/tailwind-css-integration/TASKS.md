# TASKS.md

## Task 1

Inicializar npm no projeto

### Objetivo

Criar o arquivo `package.json` na raiz do projeto `src/ATP.Web/`.

### Validação

- O arquivo `src/ATP.Web/package.json` existe com conteúdo válido
- `npm ls` não exibe erros

---

## Task 2

Instalar o Tailwind CSS CLI

### Objetivo

Adicionar `@tailwindcss/cli` como devDependency via npm.

### Validação

- `package.json` contém `@tailwindcss/cli` em `devDependencies`
- O diretório `node_modules/@tailwindcss/cli` existe
- `npx @tailwindcss/cli --help` exibe a ajuda do comando

---

## Task 3

Criar arquivo de entrada do Tailwind

### Objetivo

Criar o arquivo `Styles/app.css` com a diretiva `@import "tailwindcss"` que o Tailwind usará como ponto de partida para compilar as classes utilitárias.

### Validação

- O arquivo `src/ATP.Web/Styles/app.css` existe
- O conteúdo do arquivo contém `@import "tailwindcss"`
- O diretório `Styles/` foi criado dentro de `src/ATP.Web/`

---

## Task 4

Configurar script npm de build

### Objetivo

Adicionar o script `dev` no `package.json` que executa o comando `npx @tailwindcss/cli -i ./Styles/app.css -o ./wwwroot/css/app.css --watch`.

### Validação

- `package.json` contém o script `"dev"` com o comando Tailwind configurado
- Ao executar `npm run dev`, o Tailwind inicia em modo watch sem erros
- O arquivo `wwwroot/css/app.css` é gerado com as classes utilitárias do Tailwind

---

## Task 5

Referenciar o CSS compilado no App.razor

### Objetivo

Adicionar a tag `<link rel="stylesheet" href="@Assets["css/app.css"]" />` no `<head>` do arquivo `Components/App.razor`. O link deve ser posicionado **após** o Bootstrap e **antes** do `app.css` existente para permitir sobrescrição gradual.

### Validação

- O arquivo `Components/App.razor` contém a nova referência ao `css/app.css` na posição correta
- Ao iniciar a aplicação, o CSS do Tailwind é carregado (verificar via DevTools > Network)

---

## Task 6

Verificar .gitignore para node_modules e wwwroot/css/

### Objetivo

Confirmar que `node_modules/` e `wwwroot/css/` (artefato compilado do Tailwind) estão no `.gitignore` para evitar versionamento de dependências e outputs de build.

### Validação

- O arquivo `.gitignore` na raiz do repositório contém `node_modules/`
- O arquivo `.gitignore` contém `wwwroot/css/` ou equivalente
- `git status` não mostra `node_modules/` nem `wwwroot/css/` como untracked

---

## Task 7.1

Adicionar script `build` para produção

### Objetivo

Adicionar o script `"build": "npx @tailwindcss/cli -i ./Styles/app.css -o ./wwwroot/css/app.css"` no `package.json` (sem `--watch`) para uso em CI/deploy.

### Validação

- `package.json` contém o script `"build"`
- `npm run build` gera `wwwroot/css/app.css` corretamente e o processo encerra (sem ficar em watch)

---

## Task 7.2

Testar fluxo completo

### Objetivo

Validar a integração do Tailwind CSS ponta a ponta.

### Validação

- `npm run dev` gera `wwwroot/css/app.css` sem erros
- `npm run build` gera o mesmo arquivo e encerra corretamente
- Ao aplicar uma classe utilitária (ex: `class="text-blue-500"`) em um componente Blazor, a cor do texto é renderizada corretamente
- O `@Assets["css/app.css"]` no `App.razor` é servido corretamente (status 200)
- Os estilos existentes (Bootstrap, CSS scoped, `wwwroot/app.css`) continuam funcionando sem conflitos visíveis
- O hot reload do Blazor não é afetado pela adição do Tailwind
