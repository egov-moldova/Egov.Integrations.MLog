using Egov.Integrations.MLog.Models;

namespace Egov.Integrations.MLog;

/// <summary>
/// Represents the interface for MLog client.
/// </summary>
public interface IMLogClient : IDisposable
{
    /// <summary>
    /// Register an event.
    /// </summary>
    /// <param name="jsonEvent">The event to register.</param>
    /// <returns>Registered event ID.</returns>
    string RegisterEvent(string jsonEvent);

    /// <summary>
    /// Register an event.
    /// </summary>
    /// <param name="event">The event to register.</param>
    /// <returns>Registered event ID.</returns>
    string RegisterEvent(MLogEvent @event);

    /// <summary>
    /// Register a batch of events.
    /// </summary>
    /// <param name="jsonEvents">The events to register.</param>
    /// <returns>Registered events batch ID.</returns>
    string RegisterEventBatch(IList<string> jsonEvents);

    /// <summary>
    /// Register a batch of events.
    /// </summary>
    /// <param name="events">The events to register.</param>
    /// <returns>Registered events batch ID.</returns>
    string RegisterEventBatch(IList<MLogEvent> events);

    /// <summary>
    /// Register an event.
    /// </summary>
    /// <param name="jsonEvent">The event to register.</param>
    /// <returns>Registered event ID.</returns>
    Task<string> RegisterEventAsync(string jsonEvent);

    /// <summary>
    /// Register an event.
    /// </summary>
    /// <param name="event">The event to register.</param>
    /// <returns>Registered event ID.</returns>
    Task<string> RegisterEventAsync(MLogEvent @event);

    /// <summary>
    /// Register a batch of events.
    /// </summary>
    /// <param name="jsonEvents">The events to register.</param>
    /// <returns>Registered events batch ID.</returns>
    Task<string> RegisterEventBatchAsync(IList<string> jsonEvents);

    /// <summary>
    /// Register a batch of events.
    /// </summary>
    /// <param name="events">The events to register.</param>
    /// <returns>Registered events batch ID.</returns>
    Task<string> RegisterEventBatchAsync(IList<MLogEvent> events);

    /// <summary>
    /// Query events by time range.
    /// </summary>
    /// <param name="from">Start time for period to search (inclusive)</param>
    /// <param name="to">End time for period to search (exclusive)</param>
    /// <param name="legalBasis">Legal basis for this search.</param>
    /// <param name="legalReason">Legal reason for this search.</param>
    /// <param name="filter">A list of key/value pairs for the known fields to search.</param>
    /// <param name="page">The page number to be returned in case there is more than 1 page in result. Default: 0 (first page).</param>
    /// <param name="pageSize">The chosen page size. Default: 50</param>
    /// <returns>Events from specified time range</returns>
    string QueryEvents(DateTime from, DateTime to, string legalBasis, string? legalReason = null, Dictionary<string, string>? filter = null, uint? page = null, uint? pageSize = null);

    /// <summary>
    /// Query event by UID
    /// </summary>
    /// <param name="uid">Event UID provided by MLog at registration time.</param>
    /// <param name="legalBasis">Legal basis for this search.</param>
    /// <param name="legalReason">Legal reason for this search.</param>
    /// <returns>Event with corresponding UID.</returns>
    string QueryEvents(string uid, string legalBasis, string? legalReason = null);

    /// <summary>
    /// Query events by time range.
    /// </summary>
    /// <param name="from">Start time for period to search (inclusive)</param>
    /// <param name="to">End time for period to search (exclusive)</param>
    /// <param name="legalBasis">Legal basis for this search.</param>
    /// <param name="legalReason">Legal reason for this search.</param>
    /// <param name="filter">A list of key/value pairs for the known fields to search.</param>
    /// <param name="page">The page number to be returned in case there is more than 1 page in result. Default: 0 (first page).</param>
    /// <param name="pageSize">The chosen page size. Default: 50</param>
    /// <returns>Events from specified time range</returns>
    Task<string> QueryEventsAsync(DateTime from, DateTime to, string legalBasis, string? legalReason = null, Dictionary<string, string>? filter = null, uint? page = null, uint? pageSize = null);

    /// <summary>
    /// Query event by UID
    /// </summary>
    /// <param name="uid">Event UID provided by MLog at registration time.</param>
    /// <param name="legalBasis">Legal basis for this search.</param>
    /// <param name="legalReason">Legal reason for this search.</param>
    /// <returns>Event with corresponding UID.</returns>
    Task<string> QueryEventsAsync(string uid, string legalBasis, string? legalReason = null);
}