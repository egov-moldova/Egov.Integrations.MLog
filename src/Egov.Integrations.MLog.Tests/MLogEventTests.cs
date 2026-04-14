using Egov.Integrations.MLog.Models;
using System.Text.RegularExpressions;

namespace Egov.Integrations.MLog.Tests;

public class MLogEventTests
{
    [Fact]
    public void Constructor_SetsDefaultValues()
    {
        // Arrange
        var eventType = "test.event";

        // Act
        var mlogEvent = new MLogEvent(eventType);

        // Assert
        Assert.Equal(eventType, mlogEvent.EventType);
        Assert.True((DateTime.Now - mlogEvent.EventTime).TotalSeconds < 5);
    }

    [Fact]
    public void Properties_SetAndGetCorrectly()
    {
        // Arrange
        var mlogEvent = new MLogEvent("test.event");
        var eventId = Guid.NewGuid().ToString();
        var correlation = "corr-123";
        var message = "Test message";
        var level = "INFO";
        var user = "user-789";
        var time = new DateTime(2026, 4, 10, 10, 0, 0, DateTimeKind.Utc);

        // Act
        mlogEvent.EventID = eventId;
        mlogEvent.EventCorrelation = correlation;
        mlogEvent.EventMessage = message;
        mlogEvent.EventLevel = level;
        mlogEvent.User = user;
        mlogEvent.EventTime = time;

        // Assert
        Assert.Equal(eventId, mlogEvent.EventID);
        Assert.Equal(correlation, mlogEvent.EventCorrelation);
        Assert.Equal(message, mlogEvent.EventMessage);
        Assert.Equal(level, mlogEvent.EventLevel);
        Assert.Equal(user, mlogEvent.User);
        Assert.Equal(time, mlogEvent.EventTime);
    }

    [Fact]
    public void ToString_SerializesPropertiesToJson()
    {
        // Arrange
        var mlogEvent = new MLogEvent("test.event")
        {
            EventID = "123",
            EventMessage = "Hello \"World\"",
            EventLevel = "DEBUG"
        };
        // Fixed time for deterministic testing
        var fixedTime = new DateTime(2026, 4, 10, 10, 0, 0, DateTimeKind.Utc);
        mlogEvent.EventTime = fixedTime;

        // Act
        var json = mlogEvent.ToString();

        // Assert
        Assert.StartsWith("{", json);
        Assert.EndsWith("}", json);
        Assert.Contains("\"event_type\":\"test.event\"", json);
        Assert.Contains("\"event_id\":\"123\"", json);
        Assert.Contains("\"event_message\":\"Hello \\\"World\\\"\"", json);
        Assert.Contains("\"event_level\":\"DEBUG\"", json);
        Assert.Contains($"\"event_time\":\"{fixedTime:O}\"", json);
    }

    [Fact]
    public void ToString_HandlesListsAndDictionaries()
    {
        // Arrange
        var mlogEvent = new MLogEvent("test.event");
        var list = new List<string> { "item1", "item2" };
        var dict = new Dictionary<string, object>
        {
            { "key1", "val1" },
            { "key2", 123 }
        };

        // Act
        mlogEvent.SetProperty("my_list", list);
        mlogEvent.SetProperty("my_dict", dict);
        var json = mlogEvent.ToString();

        // Assert
        Assert.Contains("\"my_list\":[\"item1\",\"item2\"]", json);
        // Dictionaries are serialized as objects
        Assert.Contains("\"my_dict\":{", json);
        Assert.Contains("\"key1\":\"val1\"", json);
        Assert.Contains("\"key2\":123", json);
    }

    [Fact]
    public void SetProperty_NullOrEmpty_RemovesProperty()
    {
        // Arrange
        var mlogEvent = new MLogEvent("test.event");
        mlogEvent.EventMessage = "Some message";

        // Act
        mlogEvent.EventMessage = null;
        var jsonAfterNull = mlogEvent.ToString();

        mlogEvent.EventMessage = "New message";
        mlogEvent.EventMessage = "  "; // whitespace
        var jsonAfterWhitespace = mlogEvent.ToString();

        // Assert
        Assert.DoesNotContain("\"event_message\"", jsonAfterNull);
        Assert.DoesNotContain("\"event_message\"", jsonAfterWhitespace);
    }
}
