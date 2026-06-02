using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Infrastructure.AI;

public class GeminiOptions
{
    public const string SectionName = "Gemini";
    public string ApiKey { get; init; } = string.Empty;
    public string Model { get; init; } = "gemini-2.0-flash";
    public string BaseUrl { get; init; } = "https://generativelanguage.googleapis.com";
}

public class GeminiProvider : IAIProvider
{
    private readonly HttpClient _httpClient;
    private readonly GeminiOptions _options;
    private readonly ILogger<GeminiProvider> _logger;

    public GeminiProvider(HttpClient httpClient, IOptions<GeminiOptions> options, ILogger<GeminiProvider> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken)
    {
        var model = _options.Model;
        var promptLength = prompt.Length;

        _logger.LogInformation("Enviando prompt para Gemini. Modelo: {Model}, Tamanho: {Length} caracteres", model, promptLength);

        var request = new GeminiRequest
        {
            Contents =
            [
                new GeminiContent
                {
                    Parts = [new GeminiPart { Text = prompt }]
                }
            ]
        };

        var url = $"{_options.BaseUrl.TrimEnd('/')}/v1beta/models/{model}:generateContent?key={_options.ApiKey}";

        var startTime = DateTime.UtcNow;

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(url, request, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha na comunicação com Gemini. Modelo: {Model}", model);
            throw;
        }

        var elapsed = DateTime.UtcNow - startTime;
        _logger.LogInformation("Resposta recebida do Gemini. Status: {StatusCode}, Tempo: {ElapsedMs}ms",
            (int)response.StatusCode, elapsed.TotalMilliseconds);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Gemini retornou erro. Status: {StatusCode}, Corpo: {ErrorBody}", (int)response.StatusCode, error);
            throw new HttpRequestException($"Gemini API error {(int)response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);

        var content = result?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? string.Empty;
        _logger.LogInformation("Prompt processado com sucesso. Tamanho da resposta: {Length} caracteres", content.Length);

        return content;
    }
}

internal class GeminiRequest
{
    [JsonPropertyName("contents")]
    public GeminiContent[] Contents { get; set; } = [];
}

internal class GeminiContent
{
    [JsonPropertyName("parts")]
    public GeminiPart[] Parts { get; set; } = [];
}

internal class GeminiPart
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;
}

internal class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public GeminiCandidate[]? Candidates { get; set; }
}

internal class GeminiCandidate
{
    [JsonPropertyName("content")]
    public GeminiContent? Content { get; set; }
}
