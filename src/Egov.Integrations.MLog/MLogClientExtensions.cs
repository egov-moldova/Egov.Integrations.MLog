using System.Net.Security;
using Egov.Extensions.Configuration;
using Egov.Integrations.MLog;
using Egov.Integrations.MLog.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extensions methods to add MLog client implementation to an application.
/// </summary>
public static class MLogClientExtensions
{
    /// <summary>
    /// Adds MLog client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMLogClient(this IServiceCollection services)
        => services.AddMLogClient(_ => { });

    /// <summary>
    /// Adds MLog client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="config">The configuration being bound to.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMLogClient(this IServiceCollection services, IConfiguration config)
        => services.AddMLogClient(config, _ => { });

    /// <summary>
    /// Adds MLog client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="MLogClientOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMLogClient(this IServiceCollection services, Action<MLogClientOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddOptions<MLogClientOptions>()
            .Configure<IOptions<SystemCertificateOptions>>((mlogClientOptions, systemCertificateOptions) =>
            {
                var systemCertificateOptionsValue = systemCertificateOptions.Value;
                mlogClientOptions.SystemCertificate ??= systemCertificateOptionsValue.Certificate;
                mlogClientOptions.SystemCertificateIntermediaries = systemCertificateOptionsValue.IntermediateCertificates;
            });

        services.AddHttpClient(MLogClient.MLogHttpClientName)
            .ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var clientOptions = provider.GetRequiredService<IOptions<MLogClientOptions>>().Value;
                return new SocketsHttpHandler
                {
                    MaxConnectionsPerServer = 8,
                    SslOptions = new SslClientAuthenticationOptions
                    {
                        ClientCertificateContext = SslStreamCertificateContext.Create(clientOptions.SystemCertificate!, clientOptions.SystemCertificateIntermediaries, true)
                    }
                };
            });

        return services.AddSingleton<IMLogClient, MLogClient>();
    }

    /// <summary>
    /// Adds MLog client implementation.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add the client to.</param>
    /// <param name="config">The configuration being bound to.</param>
    /// <param name="configureOptions">A delegate to configure <see cref="MLogClientOptions"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddMLogClient(this IServiceCollection services, IConfiguration config, Action<MLogClientOptions> configureOptions)
    {
        services.Configure<MLogClientOptions>(config);
        return services.AddMLogClient(configureOptions);
    }
}