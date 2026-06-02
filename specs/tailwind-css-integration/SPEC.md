# SPEC.md

## Feature

Integração do Tailwind CSS

---

## Objetivo

Adicionar o Tailwind CSS como framework de estilização no projeto, substituindo gradativamente o uso de CSS puro e Bootstrap estático, agilizando o desenvolvimento de interfaces consistentes.

---

## Requisitos de negócio

- O Tailwind CSS deve ser instalado via npm como dependência de desenvolvimento
- Deve existir um arquivo de entrada `Styles/app.css` com as diretivas `@tailwind` para geração do CSS compilado
- O build do Tailwind deve monitorar mudanças e gerar o arquivo de saída em `wwwroot/css/app.css` automaticamente
- O arquivo CSS gerado deve ser referenciado no layout principal (`App.razor`)
- O CSS compilado deve ser servido como asset estático pelo Blazor
- O arquivo `wwwroot/app.css` existente deve ser mantido ou substituído conforme decisão técnica, sem quebrar o funcionamento atual

---

## Restrições

- O Tailwind deve ser configurado como ferramenta de build apenas (devDependency), sem impacto no runtime do .NET
- O comando de build deve ser executável via npm scripts
- O fluxo de assets estáticos do Blazor (`MapStaticAssets`) não deve ser interrompido
- Deve ser possível manter CSS scoped (`.razor.css`) paralelamente ao Tailwind sem conflitos

---

## Critérios de Aceitação

- `npm run dev` (ou script equivalente) inicia o watch do Tailwind e gera `wwwroot/css/app.css` com sucesso
- O arquivo `wwwroot/css/app.css` contém as classes utilitárias do Tailwind compiladas
- A página principal do Blazor carrega o CSS do Tailwind corretamente (visualmente verificável)
- Uma classe utilitária do Tailwind (ex: `text-blue-500`) aplicada a um elemento é renderizada com o estilo correto
- O hot reload do Blazor continua funcionando após a introdução do Tailwind
