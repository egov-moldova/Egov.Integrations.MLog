using System.Security.Cryptography.X509Certificates;

namespace Egov.Integrations.MLog.Models;

/// <summary>
/// Options for MLog client.
/// </summary>
public class MLogClientOptions
{
    /// <summary>
    /// Base address of MLog service (i.e. https://mlog.gov.md:8443).
    /// </summary>
    public Uri BaseAddress { get; set; } = default!;

    /// <summary>
    /// The HTTPS client certificate to use to authenticate with MLog API. Can be set directly, if needed.
    /// </summary>
    public X509Certificate2? SystemCertificate { get; set; }

    /// <summary>
    /// The intermediate certificates to use as HTTPS client certificate chain to authenticate with MLog API.
    /// </summary>
    public X509Certificate2Collection? SystemCertificateIntermediaries { get; set; }
}