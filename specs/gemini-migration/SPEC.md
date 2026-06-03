# SPEC.md

## Feature

Substituir OpenAI pelo Gemini

---

## Objetivo

Migrar o provedor de IA ativo do sistema de OpenAI para Gemini, mantendo a mesma funcionalidade de geração de planos de viagem sem alterar a experiência do usuário final.

---

## Requisitos de negócio

- O sistema deve utilizar o Gemini como provedor de IA padrão para geração de planos de viagem
- A experiência do usuário final não deve ser alterada pela migração
- O sistema deve preservar a capacidade de alternar entre provedores de IA no futuro via configuração
- A chave de API do Gemini deve ser configurável externamente (appsettings.json / variáveis de ambiente)
- O nome do provedor e do modelo utilizado devem ser registrados no plano de viagem gerado para rastreabilidade

---

## Restrições

- A interface `IAIProvider` não pode ser alterada
- Os casos de uso (`GenerateTravelPlan`, `RegenerateTravelPlan`) não devem ser modificados
- O código do provedor OpenAI deve permanecer no repositório para referência e possível reativação futura
- Todos os testes existentes devem continuar passando
- A migração não pode quebrar o fluxo de registro de dependências já estabelecido

---

## Critérios de Aceitação

- O Gemini é o provedor ativo após a migração
- A aplicação gera planos de viagem normalmente usando o Gemini
- A seção `Gemini` está presente no `appsettings.json` com `ApiKey` e `Model` configuráveis
- A seção `OpenAI` é removida do `appsettings.json`
- Os campos `AiProvider` e `Model` do `TravelPlan` são preenchidos corretamente
- Nenhum caso de uso existente foi alterado
- Todos os testes automatizados continuam passando
