namespace LetterGenApp.Models;

public class LetterRequest
{
    public string LetterType { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string RecipientName { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Tone { get; set; } = "Professional";
    public string AdditionalDetails { get; set; } = string.Empty;
    public string CustomPrompt { get; set; } = string.Empty;
}

public class LetterResult
{
    public string GeneratedLetter { get; set; } = string.Empty;
    public LetterRequest OriginalRequest { get; set; } = new();
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class AnthropicRequest
{
    public string model { get; set; } = "claude-sonnet-4-20250514";
    public int max_tokens { get; set; } = 1500;
    public List<AnthropicMessage> messages { get; set; } = new();
}

public class AnthropicMessage
{
    public string role { get; set; } = string.Empty;
    public string content { get; set; } = string.Empty;
}

public class AnthropicResponse
{
    public List<AnthropicContent> content { get; set; } = new();
    public string? error { get; set; }
}

public class AnthropicContent
{
    public string type { get; set; } = string.Empty;
    public string text { get; set; } = string.Empty;
}
