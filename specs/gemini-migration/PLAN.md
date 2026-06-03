# PLAN.md

## Existing Context

### Architecture

Feature-based Clean Architecture (Monolito Modular)

### Stack

- C# (.NET 10)
- ASP.NET Core with Blazor Server
- Entity Framework Core 10.0.8 (SQLite)
- xUnit + Moq para testes

### Existing Conventions

- Interface `IAIProvider` como abstração para provedores de IA
- `IOptions<T>` + Options classes para configuração
- `AddHttpClient<IAIProvider, TProvider>()` para registro do provedor
- Casos de uso como classes scoped injetadas diretamente
- `PromptBuilder.Build(Trip)` para construção de prompts
- `TravelPlan` com campos `AiProvider` e `Model` para rastreabilidade

---

## Arquitetura

A arquitetura existente permanece inalterada. A interface `IAIProvider` continua sendo a abstração entre os casos de uso e o provedor de IA. A única mudança é a implementação concreta registrada no contêiner DI, que passa de `OpenAiProvider` para `GeminiProvider`.

O `GeminiProvider` já está implementado e segue os mesmos padrões do `OpenAiProvider` (HttpClient, IOptions, logging).

---

## Estrutura Técnica

### Entidades

- `TravelPlan` (já existe — nenhuma alteração necessária)

### Serviços

- `GeminiProvider` (já existe — apenas registrar no DI)
- `OpenAiProvider` (já existe — manter no código, remover do DI)

### DTOs

- `GeminiRequest`, `GeminiContent`, `GeminiPart`, `GeminiResponse`, `GeminiCandidate` (já existem em `Infrastructure/AI/GeminiProvider.cs`)
- `GeminiOptions` (já existe — seção `"Gemini"`)

### Repositórios

- Nenhuma alteração necessária

### Endpoints

- Nenhuma alteração necessária (Blazor Server, sem endpoints REST)

---

## Configuração

### appsettings.json

Substituir:

```json
"OpenAI": {
    "ApiKey": "",
    "Model": "gpt-4o-mini"
}
```

Por:

```json
"Gemini": {
    "ApiKey": "",
    "Model": "gemini-2.0-flash"
}
```

---

## Decisões Técnicas

- **Manter OpenAiProvider.cs**: O código do provedor OpenAI permanece no projeto para referência e possível reativação futura. Apenas o registro no DI é removido.
- **População dos campos AiProvider e Model**: Os casos de uso (`GenerateTravelPlan`, `RegenerateTravelPlan`) devem ser ajustados para preencher `TravelPlan.AiProvider` e `TravelPlan.Model` com os valores corretos. Como o `IAIProvider` não expõe essas informações, a abordagem recomendada é:
  - Opção A: Adicionar `(string ProviderName, string ModelName)` ao retorno de `IAIProvider.GenerateAsync` — **rejeitada** por violar a restrição de não alterar a interface.
  - Opção B: Injtar `GeminiOptions` diretamente nos casos de uso — **rejeitada** por acoplar os casos de uso a uma implementação concreta.
  - Opção C: Extrair os valores de `GeminiOptions` em `Program.cs` e passá-los como constantes/scoped values — **rejeitada** por complexidade desnecessária.
  - **Opção D (escolhida)**: Registrar um `IGeminiMetadata` simples que expõe `ProviderName` e `ModelName`, injetável nos casos de uso sem violar o desacoplamento do `IAIProvider`.

### Assumptions

- `GeminiProvider` e `OpenAiProvider` utilizam o mesmo padrão de HttpClient e logging
- O modelo padrão do Gemini é `gemini-2.0-flash`
- A chave da API Gemini deve ser fornecida via `appsettings.json`, variável de ambiente ou Docker secret
