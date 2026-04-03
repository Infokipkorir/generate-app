# LetterGen AI — ASP.NET Core Letter Generator

An AI-powered letter generation web app built with ASP.NET Core MVC and the Anthropic Claude API.

## Features
- Quick Builder form (letter type, tone, sender/recipient, purpose)
- Custom free-text prompt mode
- Copy to clipboard and print support
- Regenerate button to try a different version
- Clean, responsive UI

## Setup

### 1. Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An [Anthropic API key](https://console.anthropic.com/)

### 2. Add your API key
Open `appsettings.json` and replace the placeholder:

```json
{
  "Anthropic": {
    "ApiKey": "sk-ant-..."
  }
}
```

> For production, use environment variables or secrets:
> ```bash
> dotnet user-secrets set "Anthropic:ApiKey" "sk-ant-..."
> ```

### 3. Run the app
```bash
cd LetterGenApp
dotnet run
```
Then open https://localhost:5001 in your browser.

## Project Structure

```
LetterGenApp/
├── Controllers/
│   └── LetterController.cs     # HTTP routes
├── Models/
│   └── LetterModels.cs         # Request/response models + API DTOs
├── Services/
│   └── LetterService.cs        # Anthropic API integration
├── Views/
│   ├── Letter/
│   │   ├── Index.cshtml        # The prompt / builder form
│   │   └── Result.cshtml       # Generated letter display
│   └── Shared/
│       └── _Layout.cshtml      # Shared layout + nav
├── wwwroot/css/site.css        # Stylesheet
├── Program.cs                  # App startup & DI
├── appsettings.json            # Config (add API key here)
└── LetterGenApp.csproj
```

## Customisation tips
- Add more letter types in `Views/Letter/Index.cshtml`
- Change the Claude model in `Models/LetterModels.cs` (`AnthropicRequest.model`)
- Adjust `max_tokens` for longer/shorter output
- Add a database (e.g. EF Core + SQLite) to save generated letters
