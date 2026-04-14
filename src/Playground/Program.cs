using Egov.Integrations.MLog;
using Egov.Integrations.MLog.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSystemCertificate(builder.Configuration.GetSection("Certificate"));

builder.Services.AddMLogClient(options =>
{
    options.BaseAddress = new Uri("https://mlog.staging.egov.md:8443");
});

var services = builder.Services.BuildServiceProvider();
var client = services.GetRequiredService<IMLogClient>();

var id = client.RegisterEvent(new MLogEvent("test.mlog.library")
{
    EventID = Guid.NewGuid().ToString("D"),
    EventCorrelation = Guid.NewGuid().ToString("D"),
    EventTime = DateTime.Now,
    EventMessage = "Another message",
    EventLevel = "INFO"
});

Console.WriteLine($"Log registered with {id}");