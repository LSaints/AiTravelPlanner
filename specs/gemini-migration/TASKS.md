# TASKS.md

## Task 1

Atualizar appsettings.json com configuração do Gemini

### Objetivo

Substituir a seção `OpenAI` pela seção `Gemini` no arquivo de configuração da aplicação.

### Validação

- O arquivo `appsettings.json` contém a seção `Gemini` com as chaves `ApiKey` e `Model`
- A seção `OpenAI` não está mais presente

---

## Task 2

Criar IGeminiMetadata para exporProviderName e ModelName

### Objetivo

Criar uma interface e implementação que exponha o nome do provedor e do modelo atual, permitindo que os casos de uso populem os campos `TravelPlan.AiProvider` e `TravelPlan.Model` sem depender diretamente do `GeminiOptions`.

### Validação

- `IGeminiMetadata` existe em `Shared/Abstractions/` (ou `Infrastructure/AI/`)
- A interface expõe `string ProviderName { get; }` e `string ModelName { get; }`
- A implementação `GeminiMetadata` lê os valores de `GeminiOptions`
- `GeminiMetadata` está registrado no DI como singleton ou scoped

---

## Task 3

Registrar GeminiProvider e GeminiMetadata no Program.cs

### Objetivo

Substituir o registro do `OpenAiProvider` pelo `GeminiProvider` no contêiner DI e registrar o `GeminiMetadata`.

### Validação

- `AddHttpClient<IAIProvider, GeminiProvider>()` está presente
- `builder.Services.Configure<GeminiOptions>(...)` está presente
- `services.AddSingleton<IGeminiMetadata, GeminiMetadata>()` está presente
- O registro do `OpenAiOptions` e `AddHttpClient<IAIProvider, OpenAiProvider>` foi removido

---

## Task 4

Popular campos AiProvider e Model nos casos de uso

### Objetivo

Ajustar `GenerateTravelPlan` e `RegenerateTravelPlan` para preencher `TravelPlan.AiProvider` e `TravelPlan.Model` usando `IGeminiMetadata`.

### Validação

- `GenerateTravelPlan` preenche `AiProvider` e `Model` com os valores do metadata
- `RegenerateTravelPlan` preenche `AiProvider` e `Model` com os valores do metadata
- Nenhuma alteração na interface `IAIProvider`

---

## Task 5

Verificar que os testes existentes continuam passando

### Objetivo

Garantir que a migração não quebrou nenhum teste existente.

### Validação

- `dotnet test` executa sem falhas
- Todos os testes do `PromptBuilderTests` passam
- Todos os testes de `GenerateTravelPlanTests` e `RegenerateTravelPlanTests` passam

---

## Task 6

Validar o fluxo completo de geração de plano de viagem

### Objetivo

Verificar ponta a ponta que a aplicação gera planos de viagem usando o Gemini.

### Validação

- A aplicação inicia sem erros
- Um novo plano de viagem é gerado com sucesso
- O `TravelPlan` gerado contém:
  - `AiProvider` = `"Gemini"`
  - `Model` = `"gemini-2.0-flash"` (ou o modelo configurado)
  - `Status` = `Completed`
  - `GeneratedContent` preenchido com a resposta do Gemini
