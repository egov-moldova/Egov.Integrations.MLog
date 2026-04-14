# Egov.Integrations.MLog

[![NuGet Version](https://img.shields.io/nuget/v/Egov.Integrations.MLog.svg)](https://www.nuget.org/packages/Egov.Integrations.MLog)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A production-ready C# library for integrating with MLog services. This library provides a clean and easy-to-use client to register and query events using secure certificate-based authentication, designed for modern cloud-native and microservices architectures.

---

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
  - [Registering an Event](#registering-an-event)
  - [Querying Events](#querying-events)
- [Testing](#testing)
- [Contributing](#contributing)
- [Code of Conduct](#code-of-conduct)
- [AI Assistance](#ai-assistance)
- [License](#license)

---

## Features

- **Simple Event Registration**: Register single events or batches of events with a rich metadata model.
- **Advanced Querying**: Search for events by UID or time range with customizable filtering.
- **Secure Authentication**: Built-in support for mutual TLS (mTLS) using certificate-based authentication.
- **Dependency Injection**: Seamless integration with standard .NET dependency injection (`IServiceCollection`).
- **Asynchronous API**: Native support for `async`/`await` patterns for non-blocking I/O.
- **Modern .NET**: Built for .NET 10+ leveraging the latest runtime improvements.

---

## Prerequisites

- .NET 10.0 or later
- Valid MLog client certificate for authentication

---

## Installation

Install the package via NuGet:

```bash
dotnet add package Egov.Integrations.MLog
```

---

## Configuration

The library uses `MLogClientOptions` for configuration. You can configure it via `appsettings.json` or in code.

### appsettings.json

```json
{
  "MLog": {
    "BaseAddress": "https://mlog.staging.egov.md:8443"
  },
  "Certificate": {
    "Path": "path/to/your/certificate.pfx",
    "Password": "your_password"
  }
}
```

### Dependency Injection Setup

Register the MLog client in your **Program.cs**:

```csharp
using Egov.Integrations.MLog;

var builder = WebApplication.CreateBuilder(args);

// Add certificate support (from Age.Extensions.Configuration)
builder.Services.AddSystemCertificate(builder.Configuration.GetSection("Certificate"));

// Add MLog Client
builder.Services.AddMLogClient(builder.Configuration.GetSection("MLog"));

var app = builder.Build();
```

---

## Usage

### Registering an Event

Use the `IMLogClient` to register events fluently:

```csharp
using Egov.Integrations.MLog;
using Egov.Integrations.MLog.Models;

public class MyService(IMLogClient mlogClient)
{
    public async Task LogSomethingAsync()
    {
        var mlogEvent = new MLogEvent("my.app.event")
        {
            EventID = Guid.NewGuid().ToString("D"),
            EventCorrelation = "corr-123",
            EventMessage = "Something important happened",
            EventLevel = "INFO",
            EventTime = DateTime.UtcNow,
            User = "user-id-789"
        };

        string eventId = await mlogClient.RegisterEventAsync(mlogEvent);
        Console.WriteLine($"Event registered with ID: {eventId}");
    }
}
```

### Querying Events

Query events based on time ranges and legal context:

```csharp
public async Task SearchEventsAsync(IMLogClient mlogClient)
{
    var from = DateTime.UtcNow.AddDays(-1);
    var to = DateTime.UtcNow;
    var legalBasis = "Law 123/2023, Art 5";

    string results = await mlogClient.QueryEventsAsync(from, to, legalBasis);
    Console.WriteLine(results);
}
```

---

## Testing

The solution includes a comprehensive test suite using xUnit.

### Running the tests

```bash
dotnet test src/Egov.Integrations.MLog.sln
```

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines on how to get started.

---

## Code of Conduct

This project adheres to the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you are expected to uphold this code.

---

## AI Assistance

This repository contains an [AGENTS.md](AGENTS.md) file with instructions and context for AI coding agents to assist in development, ensuring consistency in code style and project structure.

---

## License

This project is licensed under the [MIT License](LICENSE).
