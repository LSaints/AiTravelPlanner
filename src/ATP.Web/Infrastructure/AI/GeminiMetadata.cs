using Microsoft.Extensions.Options;
using ATP.Web.Shared.Abstractions;

namespace ATP.Web.Infrastructure.AI;

public class GeminiMetadata : IGeminiMetadata
{
    public string ProviderName => "Gemini";
    public string ModelName { get; }

    public GeminiMetadata(IOptions<GeminiOptions> options)
    {
        ModelName = options.Value.Model;
    }
}
