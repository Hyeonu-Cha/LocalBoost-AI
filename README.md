
# LocalBoost AI

This project is an AI-powered content generator for local businesses, built with .NET and Blazor.

## Prerequisites

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Azure Subscription](https://azure.microsoft.com/free/)
*   [Azure OpenAI Service](https://azure.microsoft.com/services/openai-service/)
*   [Bing Search API Key](https://www.microsoft.com/bing/apis/bing-web-search-api)

## Configuration

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/Hyeonu-Cha/LocalBoost-AI.git
    cd LocalBoost-AI
    ```

2.  **Configure API Keys:**

    Open the `LocalBoostAI.WorkerService/appsettings.json` file and fill in your API keys and endpoints:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      },
      "AzureOpenAI": {
        "Endpoint": "YOUR_AZURE_OPENAI_ENDPOINT",
        "ApiKey": "YOUR_AZURE_OPENAI_API_KEY",
        "DeploymentName": "YOUR_AZURE_OPENAI_DEPLOYMENT_NAME"
      },
      "BingSearch": {
        "ApiKey": "YOUR_BING_SEARCH_API_KEY"
      }
    }
    ```

## Running the Application

You need to run both the Web App and the Worker Service simultaneously.

1.  **Run the Web App:**

    Open a terminal and navigate to the `LocalBoostAI.WebApp` directory:

    ```bash
    cd LocalBoostAI.WebApp
    dotnet run
    ```

    The web application will be available at `https://localhost:5001` (or a similar address).

2.  **Run the Worker Service:**

    Open another terminal and navigate to the `LocalBoostAI.WorkerService` directory:

    ```bash
    cd LocalBoostAI.WorkerService
    dotnet run
    ```

    The worker service will start and run in the background, generating content once a day.

## How to Use

1.  Open the web application in your browser.
2.  Navigate to the **Profile** page and fill in your business details.
3.  The worker service will generate content for you within 24 hours.
4.  Navigate to the **Content Review** page to see the generated content.
5.  You can edit the content and save your changes.
