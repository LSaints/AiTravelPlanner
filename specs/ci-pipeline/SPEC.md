# SPEC.md

## Feature

Pipeline de CI para GitHub

---

## Objetivo

Automatizar validação de qualidade do código via GitHub Actions sempre que houver um Pull Request ou Push para o repositório, garantindo que dependências sejam restauradas, o projeto compile, os assets sejam empacotados e os testes passem antes de qualquer merge.

---

## Requisitos de negócio

- Executar pipeline automaticamente em todo Push para branches principais (main, master) e em todo Pull Request aberto contra essas branches
- Restaurar todas as dependências do projeto (.NET e NPM) antes do build
- Compilar o projeto e verificar se não há erros de sintaxe ou referência
- Processar assets CSS (Tailwind) como parte do build
- Executar a suíte de testes automatizados (unitários) e reportar falhas
- Falhar a pipeline inteira caso qualquer etapa apresente erro
- Notificar o desenvolvedor visualmente no GitHub (status check) sobre o resultado da pipeline

---

## Restrições

- Pipeline deve ser definida exclusivamente via YAML do GitHub Actions dentro do diretório `.github/workflows/`
- Deve utilizar a versão estável mais recente do .NET SDK disponível no GitHub Actions (compatível com net10.0)
- Deve utilizar a versão LTS do Node.js para execução do Tailwind CLI
- Não deve expor ou hardcodar secrets no arquivo de workflow
- A pipeline não deve exceder 10 minutos de execução total
- O custo de execução deve ser zero (utilizar runners públicos do GitHub)

---

## Critérios de Aceitação

- Um Push para main ou master aciona a pipeline automaticamente
- Um Push para uma branch não protegida aciona a pipeline
- Um Pull Request aberto contra main/master aciona a pipeline
- A pipeline executa checkout, restore, build, build dos assets e testes em sequência
- A pipeline falha se o build quebrar
- A pipeline falha se algum teste falhar
- O status check (sucesso/falha) aparece no PR e no commit
- A pipeline completa em menos de 10 minutos
- É possível identificar qual etapa falhou diretamente na interface do GitHub Actions
