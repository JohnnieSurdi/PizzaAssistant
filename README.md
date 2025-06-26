Important to add appsettings locally to have:
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AzureSpeech": {
    "SubscriptionKey": "YOUR-KEY",
    "Region": "westeurope"
  },
  "OpenAI": {
    "ApiKey": "YOUR-API-KEY",
    "Model": "gpt-3.5-turbo"
  },
  "Ollama": {
    "Model": "phi3:mini"
  },
  "TinyLlama": {
    "Model": "tinyllama:chat"
  }
}
