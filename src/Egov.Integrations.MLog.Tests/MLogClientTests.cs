using Egov.Integrations.MLog.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace Egov.Integrations.MLog.Tests;

public class MLogClientTests
{
    [Fact]
    public void RegisterEventBatch_NullOrEmpty_ThrowsArgumentException()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();
        var options = Options.Create(new MLogClientOptions { BaseAddress = new Uri("https://localhost") });
        var client = new MLogClient(mockFactory.Object, options);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => client.RegisterEventBatch((IList<MLogEvent>)null!));
        Assert.Throws<ArgumentException>(() => client.RegisterEventBatch(new List<MLogEvent>()));
    }

    [Fact]
    public async Task RegisterEventAsync_EmptyJson_ThrowsArgumentException()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();
        var options = Options.Create(new MLogClientOptions { BaseAddress = new Uri("https://localhost") });
        var client = new MLogClient(mockFactory.Object, options);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => client.RegisterEventAsync(""));
        await Assert.ThrowsAsync<ArgumentException>(() => client.RegisterEventAsync("  "));
    }
}
