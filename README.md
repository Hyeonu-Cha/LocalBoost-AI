
# LocalBoost AI

This project is an AI-powered content generator for local businesses, built with .NET and Blazor.

## Prerequisites

*   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Google Cloud SDK](https://cloud.google.com/sdk/docs/install)
*   [Google Cloud Project](https://cloud.google.com/resource-manager/docs/creating-managing-projects)
*   [Bing Search API Key](https://www.microsoft.com/bing/apis/bing-web-search-api)

## Configuration

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/Hyeonu-Cha/LocalBoost-AI.git
    cd LocalBoost-AI
    ```

2.  **Configure Google Cloud:**

    a.  **Authenticate with Google Cloud:**

        ```bash
        gcloud auth application-default login
        ```

    b.  **Set up your project:**

        Open the `LocalBoostAI.WorkerService/appsettings.json` file and fill in your Google Cloud project details:

        ```json
        {
          "Logging": {
            "LogLevel": {
              "Default": "Information",
              "Microsoft.Hosting.Lifetime": "Information"
            }
          },
          "GoogleCloud": {
            "ProjectId": "YOUR_GOOGLE_CLOUD_PROJECT_ID",
            "Location": "us-central1",
            "ModelId": "gemini-1.5-flash-001"
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
