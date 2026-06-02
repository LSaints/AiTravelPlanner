using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Infrastructure.AI;

public class OpenAiOptions
{
    public const string SectionName = "OpenAI";
    public string ApiKey { get; init; } = string.Empty;
    public string Model { get; init; } = "gpt-4o-mini";
    public string BaseUrl { get; init; } = "https://api.openai.com";
}

public class OpenAiProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly OpenAiOptions _options;
    private readonly ILogger<OpenAiProvider> _logger;

    public OpenAiProvider(HttpClient httpClient, IOptions<OpenAiOptions> options, ILogger<OpenAiProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken)
    {
        var model = _options.Model;
        var promptLength = prompt.Length;

        _logger.LogInformation("Enviando prompt para OpenAI. Modelo: {Model}, Tamanho: {Length} caracteres", model, promptLength);

        var request = new OpenAiChatRequest
        {
            Model = model,
            Messages =
            [
                new OpenAiMessage { Role = "user", Content = prompt }
            ]
        };

        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1/chat/completions";

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ApiKey);
        httpRequest.Content = JsonContent.Create(request);

        var startTime = DateTime.UtcNow;

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha na comunicação com OpenAI. Modelo: {Model}", model);
            throw;
        }

        var elapsed = DateTime.UtcNow - startTime;
        _logger.LogInformation("Resposta recebida do OpenAI. Status: {StatusCode}, Tempo: {ElapsedMs}ms",
            (int)response.StatusCode, elapsed.TotalMilliseconds);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("OpenAI retornou erro. Status: {StatusCode}, Corpo: {ErrorBody}", (int)response.StatusCode, error);
            throw new HttpRequestException($"OpenAI API error {(int)response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<OpenAiChatResponse>(cancellationToken: cancellationToken);

        var content = result?.Choices?[0]?.Message?.Content ?? string.Empty;
        _logger.LogInformation("Prompt processado com sucesso. Tamanho da resposta: {Length} caracteres", content.Length);

        return content;
    }
}

internal class OpenAiChatRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("messages")]
    public OpenAiMessage[] Messages { get; set; } = [];
}

internal class OpenAiMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

internal class OpenAiChatResponse
{
    [JsonPropertyName("choices")]
    public OpenAiChoice[]? Choices { get; set; }
}

internal class OpenAiChoice
{
    [JsonPropertyName("message")]
    public OpenAiMessage? Message { get; set; }
}
