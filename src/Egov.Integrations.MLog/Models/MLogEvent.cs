using System.Collections;
using System.Text;
using Egov.Integrations.MLog.Utils;

namespace Egov.Integrations.MLog.Models;

/// <summary>
/// Represents the properties and methods of an MLog event.
/// </summary>
public class MLogEvent
{
    private readonly Dictionary<string, object?> _properties = new();

    /// <summary>
    /// MLog event.
    /// </summary>
    /// <param name="eventType">Type of event</param>
    public MLogEvent(string eventType)
    {
        EventTime = DateTime.Now;
        EventType = eventType;
    }

    /// <summary>
    /// The moment when the event happened at the source system (i.e. not the time of the logging).
    /// </summary>
    public DateTime EventTime
    {
        get => GetProperty("event_time") as DateTime? ?? DateTime.Now;
        set => SetProperty("event_time", value);
    }

    /// <summary>
    /// The type of the event according to IS definition, which usually represents the action taken that resulted in this event (ex. Created, Authenticated, Deleted, etc.).
    /// </summary>
    public string? EventType
    {
        get => GetProperty("event_type")?.ToString();
        set => SetProperty("event_type", value);
    }

    /// <summary>
    /// An internal identifier of the event (usually unique) or some kind of internal correlation identifier unique for the logger system.
    /// </summary>
    public string? EventID
    {
        get => GetProperty("event_id")?.ToString();
        set => SetProperty("event_id", value);
    }

    /// <summary>
    /// An identifier used to correlate events logged by different systems in some context, usually a user action.
    /// </summary>
    public string? EventCorrelation
    {
        get => GetProperty("event_correlation")?.ToString();
        set => SetProperty("event_correlation", value);
    }

    /// <summary>
    /// Event classifier. Each IS which registers events to MLog can use its own definition for this, e.g. relevance (ex. high/medium/low) or impact (warning/critical/fatal), etc.
    /// </summary>
    public string? EventLevel
    {
        get => GetProperty("event_level")?.ToString();
        set => SetProperty("event_level", value);
    }

    /// <summary>
    /// Place where the event was generated
    /// </summary>
    public string? EventSource
    {
        get => GetProperty("event_source")?.ToString();
        set => SetProperty("event_source", value);
    }

    /// <summary>
    /// Free text that describes the event, indexed by MLog for advanced text search.
    /// </summary>
    public string? EventMessage
    {
        get => GetProperty("event_message")?.ToString();
        set => SetProperty("event_message", value);
    }

    /// <summary>
    /// Event details, such as an exception stack trace, document extract or other. Not indexed.
    /// </summary>
    public object? EventDetails
    {
        get => GetProperty("event_details");
        set => SetProperty("event_details", value);
    }

    /// <summary>
    /// The legal entity (organization) on behalf of which the action was performed (by a user or automatically). Usually it is the IDNO of the organization.
    /// </summary>
    public string? LegalEntity
    {
        get => GetProperty("legal_entity")?.ToString();
        set => SetProperty("legal_entity", value);
    }

    /// <summary>
    /// Legal basis for taken action.
    /// </summary>
    public object? LegalBasis
    {
        get => GetProperty("legal_basis");
        set => SetProperty("legal_basis", value);
    }

    /// <summary>
    /// Legal reason for taken action.
    /// </summary>
    public object? LegalReason
    {
        get => GetProperty("legal_reason");
        set => SetProperty("legal_reason", value);
    }

    /// <summary>
    /// The user which is the owner of the event (participated at this event creation). Usually it is the IDNP of the user.
    /// </summary>
    public string? User
    {
        get => GetProperty("user")?.ToString();
        set => SetProperty("user", value);
    }

    /// <summary>
    /// User session in which context the action happened. This attribute permits to split the action taken by a user if the action is a step of a flow.
    /// </summary>
    public string? UserSession
    {
        get => GetProperty("user_session")?.ToString();
        set => SetProperty("user_session", value);
    }

    /// <summary>
    /// User IP address or location or any another form which identifies where the user acted from.
    /// </summary>
    public string? UserAddress
    {
        get => GetProperty("user_address")?.ToString();
        set => SetProperty("user_address", value);
    }

    /// <summary>
    /// The identifier of the thing or person that is impacted, discussed, or dealt with by this event (usually an IDNP). Different from the object, as the object is directly involved in the action.
    /// </summary>
    public object? Subject
    {
        get => GetProperty("subject");
        set => SetProperty("subject", value);
    }

    /// <summary>
    /// The type of the subject.
    /// </summary>
    public object? SubjectType
    {
        get => GetProperty("subject_type");
        set => SetProperty("subject_type", value);
    }

    /// <summary>
    /// The name of the subject.
    /// </summary>
    public string? SubjectName
    {
        get => GetProperty("subject_name")?.ToString();
        set => SetProperty("subject_name", value);
    }

    /// <summary>
    /// The identifier of the thing or person to which the event action is directed.
    /// </summary>
    public object? Object
    {
        get => GetProperty("object");
        set => SetProperty("object", value);
    }

    /// <summary>
    /// The type of the object.
    /// </summary>
    public string? ObjectType
    {
        get => GetProperty("object_type")?.ToString();
        set => SetProperty("object_type", value);
    }

    /// <summary>
    /// The name of the object.
    /// </summary>
    public string? ObjectName
    {
        get => GetProperty("object_name")?.ToString();
        set => SetProperty("object_name", value);
    }

    /// <summary>
    /// Sets a property.
    /// </summary>
    /// <param name="key">Property key</param>
    /// <param name="value">Property value</param>
    public void SetProperty(string key, object? value)
    {
        if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)) || value is ICollection { Count: 0 })
        {
            _properties.Remove(key);
            return;
        }

        _properties[key] = value;
    }

    /// <summary>
    /// Reads a property.
    /// </summary>
    /// <param name="key">Property key</param>
    /// <returns></returns>
    public object? GetProperty(string key)
    {
        _properties.TryGetValue(key, out var value);
        return value;
    }

    /// <summary>
    /// Converts object to string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var result = new StringBuilder("{", _properties.Count * 32);
        foreach (var property in _properties)
        {
            if (property.Value == null) continue;

            if (result.Length > 1) result.Append(',');

            JsonUtils.AppendEscapedJsonObject(result, property.Key);
            result.Append(":");
            switch (property.Value)
            {
                case string _:
                    goto default;

                case DateTime dateTime:
                    result.Append('"');
                    result.Append(dateTime.ToString("O"));
                    result.Append('"');
                    break;

                case IEnumerable<KeyValuePair<string, object>> dictionary:
                {
                    result.Append('{');
                    var first = true;
                    foreach (var (key, value) in dictionary)
                    {
                        if (!first) result.Append(',');
                        JsonUtils.AppendEscapedJsonObject(result, key);
                        result.Append(':');
                        JsonUtils.AppendEscapedJsonObject(result, value);
                        first = false;
                    }
                    result.Append('}');
                    break;
                }

                case IEnumerable list:
                {
                    result.Append('[');
                    var first = true;
                    foreach (var item in list)
                    {
                        if (!first) result.Append(',');
                        JsonUtils.AppendEscapedJsonObject(result, item);
                        first = false;
                    }
                    result.Append(']');
                    break;
                }

                default:
                    JsonUtils.AppendEscapedJsonObject(result, property.Value);
                    break;
            }
        }
        return result.Append('}').ToString();
    }
}