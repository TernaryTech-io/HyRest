> **⚠️ DISCLAIMER**
>
> This project is an independent, third-party library and is **NOT** affiliated with, endorsed by, or supported by Hyland Software, Inc. or any of its subsidiaries. Hyland Software provides no warranty, support, or guarantees for this library. 
>
> "Hyland," "OnBase," "Unity," and related trademarks are the property of Hyland Software, Inc. This project is provided as-is under the terms of its license, and users assume all responsibility for its use. For official Hyland products and support, please visit [Hyland.com](https://www.hyland.com/).

# HyRest

**A modern, developer-friendly .NET wrapper for the Hyland OnBase REST API**

HyRest simplifies working with Hyland's OnBase REST API by providing an intuitive, strongly-typed interface that feels familiar to Unity API developers but with modern fluent api styled interactions and eventul support for dependancy injection. Say goodbye to complex HTTP calls and manual JSON serialization—HyRest handles the heavy lifting so you can focus on building great integrations.

**For working examples, check out the following projects:**
- [HyRest.Example](https://github.com/TernaryTech-io/HyRest.Example) - Simplified implementaton.
- [HyRest.OnCmd](https://github.com/TernaryTech-io/HyRest.OnCmd) - A fun terminal client for OnBase.
- [HyRest.Relay](https://github.com/TernaryTech-io/HyRest.Relay) - A relay web api utilizing the `HyRest.DependencyInjection` library, and more advanced authentication.

## Why HyRest?

- **🎯 Intuitive API** - Familiar conventions for Unity API developers transitioning to REST
- **🔒 Type-Safe** - Strongly-typed models with full IntelliSense support
- **⚡ Simplified Operations** - Complex tasks like multi-part document uploads made easy
- **🔄 Smart Keyword Handling** - Automatic type conversion and validation based on keyword type settings
- **📝 Built-in Logging** - Integrated with `Microsoft.Extensions.Logging` for comprehensive diagnostics
- **🎨 Flexible** - Choose between scoped auto-cleanup or manual session management

## Features

- ✅ Document retrieval, archiving, and content management
- ✅ Document queries (Document Type, Document Type Group, Custom Queries)
- ✅ Keyword operations (standalone, single-instance groups, multi-instance groups)
- ✅ Document reindexing
- ✅ Note management
- ✅ Authentication with Hyland Identity Server
- 🚧 Workflow operations (coming soon)
- 🚧 WorkView operations (coming soon)
- 🚧 Forms integration (coming soon)

## Documentation
- [Documentation is in progress](/docs/Documentation.md)

## Installation

### Prerequisites

- **.NET 8.0** or later
- **Hyland OnBase** with REST API enabled
- **Hyland Identity Server** configured
- Valid OnBase user credentials or API client credentials

### Install via NuGet

```bash
dotnet add package HyRest
```

Or via Package Manager Console:

```powershell
Install-Package HyRest
```

## Quick Start

Here's a complete example to get you up and running in minutes:

```csharp
using Microsoft.Extensions.Logging;
using HyRest;
using HyRest.Abstractions;


// Configure authentication
var creds = AuthenticationCredentials.CreateUserCredentials(
    username: "your-username",
    password: "your-password",
    clientId: "your-client-id",
    clientSecret: "your-client-secret"
);

// Configure client options
var options = HylandClientOptions.Create(
    idsBaseUrl: "https://onbase.server.com/IdentityServer",
    apiBaseUrl: "https://onbase.server.com/ApiServer",
    useQueryMetering: false
);

// Set up logging (optional but recommended)
var logFactory = LoggerFactory.Create(builder => 
    builder.SetMinimumLevel(LogLevel.Information).AddConsole()
);
var logger = logFactory.CreateLogger<OnBaseApp>();

// Retrieve a document using scoped approach (auto-cleanup)
using var app = OnBaseScopedApp.CreateScoped(options, creds, logger);
var doc = await app.Core.GetDocumentByIdAsync(12345);
app.Logger.LogInformation($"Retrieved document: {doc.DocumentType?.Name}");
```

## Getting Started

Check out the [HyRest.Example](https://github.com/TernaryTech-io/HyRest.Example) project for a simplified and straight forward implmentation and demonstration.

### Step 1: Creating Credentials

HyRest supports multiple authentication methods. In this example, we load credentials from environment variables using a `.env` file:

```csharp
var username = Environment.GetEnvironmentVariable("HYREST_USERNAME");
var password = Environment.GetEnvironmentVariable("HYREST_PASSWORD");
var clientId = Environment.GetEnvironmentVariable("HYREST_CLIENTID");
var clientSecret = Environment.GetEnvironmentVariable("HYREST_CLIENTSECRET");

IAuthenticationCredentials creds = AuthenticationCredentials
    .CreateUserCredentials(
        username,
        password,
        clientId,
        clientSecret
    );
```

> **Note:** Additional credential implementations are in development to support various authentication scenarios. 
**For more complex examples, including dependency injection** See the [HyRest.Relay](https://github.com/TernaryTech-io/HyRest.Relay) project utilizing the [HyRest.DependencyInject](https://github.com/TernaryTech-io/HyRest.DependencyInjection) library.

### Step 2: Configuring Client Options

Client options provide fine-grained control over your HyRest application while offering sensible defaults:

| Type | Property | Usage | Description |
| --- | --- | --- | --- |
| `string` | `IdsBaseUrl` | **REQUIRED** | Base URL of your Hyland Identity Server<br/>Example: `https://onbase.server.com/IdentityServer` |
| `string` | `ApiBaseUrl` | **REQUIRED** | Base URL of your Hyland REST API Server<br/>Example: `https://onbase.server.com/ApiServer` |
| `bool` | `UseQueryMetering` | *Optional* | Set to `true` if using QueryMetering API license<br/>Default: `false` (uses concurrent/named licenses) |
| `string` | `DefaultLanguage` | *Optional* | Default language for API responses<br/>Default: `en-US` |

#### Building the Options

```csharp
var options = new HylandClientOptions
    {
        IdsBaseUrl = "https://[server]/IdentityServer",
        ApiBaseUrl = "https://[server]/APIServer",
        //Optional
        DefaultLanguage = "en-US",
        UseQueryMetering = true // If you have the license. Default is false
    };
```

### Step 3: Setting Up Logging

HyRest uses `Microsoft.Extensions.Logging`, giving you access to numerous logging providers. Here's an example using console logging:

```csharp
var logFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Information)
        .AddConsole();
});
var logger = logFactory.CreateLogger<OnBaseApp>();

//OR if using the scoped version
var logger = logFactory.CreateLogger<OnBaseScopedApp>();

```

For enhanced console output with color support, you can use the `Ternary.Extensions.Logging` package:

```csharp
var logFactory = LoggerFactory.Create(builder =>
{
    builder
        .SetMinimumLevel(LogLevel.Information)
        .AddColorConsole(config =>
        {
            config.LogLevel = LogLevel.Information;
        });
});
var logger = logFactory.CreateLogger<OnBaseApp>();
```

> 

### Step 4: Creating a OnBaseApp Instance

HyRest offers two application patterns to suit different use cases:

#### OnBaseScopedApp

The scoped app implements `IDisposable` and `IAsyncDisposable`, automatically handling authentication and session cleanup:

```csharp
var scoped = OnBaseScopedApp.CreateScopedApp(logger, creds, options);
var doc = await scoped.Core.GetDocumentByIdAsync(12345);
// Session automatically disconnects when disposed```

**Best for:** Quick operations, single transactions, scripts, and background jobs.

#### Full Application (For Long-Running Operations)

The full `OnBaseApp` gives you complete control over session lifecycle to manage how you see fit.

```csharp
var app = OnBaseApp.Create(logger, creds, options);

if (!app.IsConnected)
    app.Session.Initiate(); //Initiates an OnBase Session and retrieves session cookie.
else if (app.IsConnected)
    app.Session.Disconnect(); //Logs out of OnBase, closing the session.
```


**Best for:** Long-running applications, batch processing, or when you need fine-grained session control.

## Common Tasks

### Retrieving Documents

HyRest uses familiar Unity API conventions for document operations:

```csharp
var doc = await app.Core.GetDocumentByIdAsync(12345);
    
// Access document properties
app.Logger.LogInformation($"Document Type: {doc.DocumentType?.Name}");
app.Logger.LogInformation($"Document Date: {doc.DocumentDate}");
    
// Work with keywords
foreach (var keyword in doc.KeywordCollection.StandAloneKeywords.ToList())
{
    var value = keyword.Values?.FirstOrDefault()?.Value ?? "None";
    app.Logger.LogInformation($"{keyword.KeywordType.Name}: {value}");
}
```

### Querying Documents

Perform Document Type, Document Type Group, or Custom Queries using the fluent `DocumentQueryBuilder`:

```csharp
var builder = app.Core.CreateDocumentQueryBuilder<DocumentTypeQueryBuilder>()
    .AddItem(105) // add the document type by either id or name
    .WithMaxResults(100)
    .AddQueryKeyword((keyword) =>
    {
        keyword.Id = "Description"; //Id or Name works
        keyword.Value = "You can get with this...";
        keyword.Operator = QueryKeywordOperator.Equal;
        keyword.Relation = QueryKeywordRelation.Or;
    }).AddQueryKeyword((keyword) =>
    {
        keyword.Id = "Description"; //Id or Name works
        keyword.Value = "...you can get with that.";
        keyword.Operator = QueryKeywordOperator.Equal;
    });
var query = builder.CreateQuery(includeItemCount: true);
if (query.ResultsCount > 0)
{
    var results = query.GetResults();
    foreach (var result in results)
    {
        var document = result.Document;
        var displayCols = result.DisplayColumns;
    }
}
```

### Downloading Document Content

Retrieve document content with support for revisions, renditions, and page ranges. It is recommended to NOT supply a filename, one will be generated.
If you supply a filename and the extension does not match the document's file extension, it will changed in order to match in the file.

```csharp
var content = doc.GetContent(); // You can specify rendition or revision, pages, etc.
content.SaveToFile(@"\\path\to\save");

content.SaveToFile(@"\\path\to\save", "OptionalFileName.tiff"); //Extension might be updated if it doesn't match.
```

### Uploading Documents

Document archiving is simplified with automatic handling of multi-part uploads and keyword type conversion:

```csharp
var docType = app.Core.DocumentTypes[105];
var importDoc = docType.CreateNewDocumentArchiveProperties();
importDoc.DocumentDate = DateTime.Today;
importDoc.WithFile(@"\\path\to\file.tiff");

var keyCollection = importDoc.KeywordCollection;

//Add Keywords
var keyword = keyCollection
    .CreateEditableKeyword("Description")
    .Add("Hello")
    .Add("World");

//Add Single Instance Keyword Record
var docSIKG = keyCollection.CreateEditableSingleInstanceRecord("Document Information");
var fileName = docSIKG
    .CreateEditableKeyword("File Name")
    .Add("file.tiff");
var batchNum = docSIKG
    .CreateEditableKeyword("Batch Number")
    .Add(13); 

//Add MultiInstance Keyword Record
var custMIKG = keyCollection.CreateEditableMultiInstanceRecord("Customer Information MIKG");
var custId = custMIKG
    .CreateEditableKeyword("Customer Id")
    .Add(12345);
var custName = custMIKG
    .CreateEditableKeyword("Customer Name")
    .Add("Ternary Tech");

//Archive Document - File Type will resolve automatically, but can also be specified.
var newDoc = importDoc.ArchiveDocument();
```

> **Smart Feature:** HyRest automatically validates keyword values against configured masks and performs data type conversion based on keyword type settings. You can pass a string representation of the value or a strongly typed version. For example, a numeric value can be passed as `"12345"` or `12345`

### Updating Keywords

Easily add, modify, or remove keywords from existing documents:

```csharp
// Create an editable keyword and add values
doc.KeywordCollection
    .CreateEditableKeyword("Description")
    .Add("This is a keyword value")
    .Add("You can add multiple values to standalone keywords");                                       

// Update existing values, if values doesn't exist, the new value will still be added.
doc.KeywordCollection
    .CreateEditableKeyword("Description")
    .Update("This is a keyword value", "This an update");

// Remove a specific value
doc.KeywordCollection
    .CreateEditableKeyword("Description")
    .Remove("This an update");

// Clear all the keywords
doc.KeywordCollection
    .CreateEditableKeyword("Description")
    .ClearValues();

doc.UpdateKeywords(); //Commit the keyword updates.
```

## Best Practices

1. **Use Scoped Apps for Simple & Quick Operations** - Let HyRest handle cleanup automatically
2. **Always Disconnect Sessions** - When using full `OnBaseApp`, ensure `DisconnectAsync()` is called or the Session will remain in OnBase.
3. **Enable Logging** - Helps diagnose issues and track API call counts
4. **Store Credentials Securely** - Use environment variables, Azure Key Vault, or similar secure storage
5. **Handle Exceptions Gracefully** - REST API calls can fail; always wrap operations in try-catch blocks

## Need More Help?

- 📖 Full API documentation (coming soon)
- 🐛 [Report issues](https://github.com/TernaryTech-io/HyRest/issues)
- 💡 [Request features](https://github.com/TernaryTech-io/HyRest/issues)
- 📧 Contact: support@ternarytech.io

## License

(c) 2026 [Ternary Tech](https://ternarytech.io)
This software is licensed under [Mozilla Public License Version 2.0](LICENSE)

[Refit](https://github.com/reactiveui/refit) is Copyright (c) by ReactiveUI 2012 - 2025
[Refit License](https://github.com/reactiveui/refit/blob/main/LICENSE)
