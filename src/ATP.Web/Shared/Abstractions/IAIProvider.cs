namespace ATP.Web.Shared.Abstractions;

public interface IAIProvider
{
    Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken);
}
