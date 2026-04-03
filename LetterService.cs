using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using LetterGenApp.Models;

namespace LetterGenApp.Services;

public interface ILetterService
{
    Task<LetterResult> GenerateLetterAsync(LetterRequest request);
}

public class LetterService : ILetterService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<LetterService> _logger;
    private const string ApiUrl = "https://api.anthropic.com/v1/messages";

    public LetterService(HttpClient httpClient, IConfiguration config, ILogger<LetterService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<LetterResult> GenerateLetterAsync(LetterRequest request)
    {
        try
        {
            var apiKey = _config["Anthropic:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                return Fail("Anthropic API key is not configured. Please add it to appsettings.json.");

            var prompt = BuildPrompt(request);

            var payload = new AnthropicRequest
            {
                messages = new List<AnthropicMessage>
                {
                    new() { role = "user", content = prompt }
                }
            };

            var json = JsonSerializer.Serialize(payload);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, ApiUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("x-api-key", apiKey);
            httpRequest.Headers.Add("anthropic-version", "2023-06-01");

            var response = await _httpClient.SendAsync(httpRequest);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Anthropic API error: {Status} {Body}", response.StatusCode, responseBody);
                return Fail($"API error ({(int)response.StatusCode}): {responseBody}");
            }

            var parsed = JsonSerializer.Deserialize<AnthropicResponse>(responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var letterText = parsed?.content?.FirstOrDefault()?.text ?? string.Empty;

            return new LetterResult
            {
                Success = true,
                GeneratedLetter = letterText,
                OriginalRequest = request
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate letter");
            return Fail($"Unexpected error: {ex.Message}");
        }
    }

    private static string BuildPrompt(LetterRequest r)
    {
        // If user supplied a fully custom prompt, use it directly
        if (!string.IsNullOrWhiteSpace(r.CustomPrompt))
            return $"{r.CustomPrompt}\n\nWrite only the letter itself — no commentary or preamble.";

        var sb = new StringBuilder();
        sb.AppendLine($"Write a {r.Tone.ToLower()} {r.LetterType} letter with the following details:");
        if (!string.IsNullOrWhiteSpace(r.SenderName))
            sb.AppendLine($"- From: {r.SenderName}");
        if (!string.IsNullOrWhiteSpace(r.RecipientName))
            sb.AppendLine($"- To: {r.RecipientName}");
        if (!string.IsNullOrWhiteSpace(r.Purpose))
            sb.AppendLine($"- Purpose: {r.Purpose}");
        if (!string.IsNullOrWhiteSpace(r.AdditionalDetails))
            sb.AppendLine($"- Additional details: {r.AdditionalDetails}");
        sb.AppendLine();
        sb.AppendLine("Write only the complete letter — no commentary, no preamble, no closing remarks outside the letter.");
        return sb.ToString();
    }

    private static LetterResult Fail(string msg) => new() { Success = false, ErrorMessage = msg };
}
