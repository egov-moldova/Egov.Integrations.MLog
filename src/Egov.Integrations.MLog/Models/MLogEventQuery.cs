namespace Egov.Integrations.MLog.Models;

/// <summary>
/// MLog query specification.
/// </summary>
public class MLogEventQuery
{
    /// <summary>
    /// Event UID.
    /// </summary>
    public string? UID { get; set; }

    /// <summary>
    /// Period start time.
    /// </summary>
    public DateTime EventTimeFrom { get; set; }

    /// <summary>
    /// Period end time.
    /// </summary>
    public DateTime EventTimeTo { get; set; }

    /// <summary>
    /// Legal basis for taken action.
    /// </summary>
    public string? LegalBasis { get; set; }

    /// <summary>
    /// Legal reason for taken action.
    /// </summary>
    public string? LegalReason { get; set; }

    /// <summary>
    /// A list of kay/value pairs for the known fields to search.
    /// </summary>
    public Dictionary<string, string>? Filter { get; set; }

    /// <summary>
    /// The page number to be returned in case there is more than 1 page in result. Default: 0 (first page).
    /// </summary>
    public uint? Page { get; set; }

    /// <summary>
    /// The chosen page size. Default: 50.
    /// </summary>
    public uint? PageSize { get; set; }

    /// <summary>
    /// Builds a query.
    /// </summary>
    /// <returns>A built query.</returns>
    /// <exception cref="ArgumentException"></exception>
    public string BuildQuery()
    {
        if (!string.IsNullOrWhiteSpace(UID))
        {
            BuildQuery(UID, LegalBasis, LegalReason);
        }
        if ((EventTimeFrom != DateTime.MinValue) && (EventTimeTo != DateTime.MinValue) && !string.IsNullOrWhiteSpace(LegalBasis))
        {
            BuildQuery(EventTimeFrom, EventTimeTo, LegalBasis, LegalReason, Filter, Page, PageSize);
        }
        throw new ArgumentException($"Either {nameof(UID)} or {nameof(EventTimeFrom)} and {nameof(EventTimeTo)} and {nameof(LegalBasis)} should be completed");
    }

    /// <summary>
    /// Builds a query with given parameters.
    /// </summary>
    /// <param name="uid">Event UID.</param>
    /// <param name="legalBasis">Legal basis for taken action.</param>
    /// <param name="legalReason">Legal reason for taken action.</param>
    /// <returns>A built query with given parameters.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static string BuildQuery(string uid, string? legalBasis = null, string? legalReason = null)
    {
        if (string.IsNullOrWhiteSpace(uid)) throw new ArgumentException($"{nameof(uid)} cannot be empty");
        var query = $"?uid={Uri.EscapeDataString(uid)}";
        if (!string.IsNullOrWhiteSpace(legalBasis)) query += $"&legal_basis={Uri.EscapeDataString(legalBasis)}";
        if (!string.IsNullOrWhiteSpace(legalReason)) query += $"&legal_reason={Uri.EscapeDataString(legalReason)}";
        return query;
    }


    /// <summary>
    /// Builds a query with given parameters.
    /// </summary>
    /// <param name="eventTimeFrom">Start time of search period.</param>
    /// <param name="eventTimeTo">End time of search period.</param>
    /// <param name="legalBasis">Legal basis for taken action.</param>
    /// <param name="legalReason">Legal reason for taken action.</param>
    /// <param name="filter">A list of key/value pairs for the known fields to search.</param>
    /// <param name="page">The page number to be returned in case there is more than 1 page in result. Default: 0 (first page).</param>
    /// <param name="pageSize">The chosen page size. Default: 50.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string BuildQuery(DateTime eventTimeFrom, DateTime eventTimeTo, string legalBasis, string? legalReason = null, Dictionary<string, string>? filter = null, uint? page = null, uint? pageSize = null)
    {
        if ((eventTimeFrom == DateTime.MinValue) || (eventTimeTo == DateTime.MinValue) || string.IsNullOrWhiteSpace(legalBasis))
        {
            throw new ArgumentException($"{nameof(eventTimeFrom)} and {nameof(eventTimeTo)} and {nameof(legalBasis)} should be completed");
        }
        var query = $"?event_time_from={eventTimeFrom:O}&event_time_to={eventTimeTo:O}&legal_basis={Uri.EscapeDataString(legalBasis)}";
        if (!string.IsNullOrWhiteSpace(legalReason)) query += $"&legal_reason={Uri.EscapeDataString(legalReason)}";
        if ((filter != null) && (filter.Count > 0))
        {
            query += "&filter=" + string.Join(",", filter.Select(x => Uri.EscapeDataString(x.Key) + "=" + Uri.EscapeDataString(x.Value)));
        }
        if (page != null) query += $"&page={page}";
        if (pageSize != null) query += $"&page_size={pageSize}";
        return query;
    }
}