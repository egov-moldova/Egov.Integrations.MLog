using Microsoft.Extensions.Options;
using System.Text;
using Egov.Integrations.MLog.Models;

namespace Egov.Integrations.MLog;

internal class MLogClient : IMLogClient
{
    public const string MLogHttpClientName = "mlog-client";
    private readonly Uri _registerUrl;
    private readonly Uri _queryUrl;
    private readonly HttpClient _client;
    private bool _disposed;

    public MLogClient(IHttpClientFactory httpClientFactory, IOptions<MLogClientOptions> options)
    {
        var optionsValue = options.Value;
        _registerUrl = new Uri(optionsValue.BaseAddress, "register");
        _queryUrl = new Uri(optionsValue.BaseAddress, "query");
        _client = httpClientFactory.CreateClient(MLogHttpClientName);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            _client.Dispose();
        }
        _disposed = true;
    }

    /**** REGISTER *********************************************************************************/

    public string RegisterEvent(string jsonEvent)
    {
        return RegisterEventAsync(jsonEvent).Result;
    }

    public string RegisterEventBatch(IList<string> jsonEvents)
    {
        if (jsonEvents == null || jsonEvents.Count == 0) throw new ArgumentException("No events provided for registration", nameof(jsonEvents));
        return RegisterEvent(string.Join(Environment.NewLine, jsonEvents));
    }

    public string RegisterEvent(MLogEvent @event)
    {
        return RegisterEvent(@event.ToString());
    }

    public string RegisterEventBatch(IList<MLogEvent> events)
    {
        if (events == null || events.Count == 0) throw new ArgumentException("No events provided for registration", nameof(events));
        return RegisterEventBatch(events.Select(x => x.ToString()).ToList());
    }

    public Task<string> RegisterEventAsync(string jsonEvent)
    {
        if (string.IsNullOrWhiteSpace(jsonEvent)) throw new ArgumentException("Cannot register empty event", nameof(jsonEvent));
        return HttpPostAsync(jsonEvent);
    }

    public Task<string> RegisterEventBatchAsync(IList<string> jsonEvents)
    {
        if (jsonEvents == null || jsonEvents.Count == 0) throw new ArgumentException("No events provided for registration", nameof(jsonEvents));
        return RegisterEventAsync(string.Join(Environment.NewLine, jsonEvents));
    }

    public Task<string> RegisterEventAsync(MLogEvent @event)
    {
        if (@event == null) throw new ArgumentException("Cannot register empty event", nameof(@event));
        return RegisterEventAsync(@event.ToString());
    }

    public Task<string> RegisterEventBatchAsync(IList<MLogEvent> events)
    {
        if (events == null || events.Count == 0) throw new ArgumentException("No events provided for registration", nameof(events));
        return RegisterEventBatchAsync(events.Select(x => x.ToString()).ToList());
    }


    /**** QUERY *********************************************************************************/

    public string QueryEvents(DateTime from, DateTime to, string legalBasis, string? legalReason = null, Dictionary<string, string>? filter = null, uint? page = null, uint? pageSize = null)
    {
        return QueryEventsAsync(from, to, legalBasis, legalReason, filter, page, pageSize).Result;
    }

    public string QueryEvents(string uid, string legalBasis, string? legalReason = null)
    {
        return QueryEventsAsync(uid, legalBasis, legalReason).Result;
    }

    public Task<string> QueryEventsAsync(DateTime from, DateTime to, string legalBasis, string? legalReason = null, Dictionary<string, string>? filter = null, uint? page = null, uint? pageSize = null)
    {
        var query = MLogEventQuery.BuildQuery(from, to, legalBasis, legalReason, filter, page, pageSize);
        return HttpGetAsync(query);
    }

    public Task<string> QueryEventsAsync(string uid, string legalBasis, string? legalReason = null)
    {
        var query = MLogEventQuery.BuildQuery(uid, legalBasis, legalReason);
        return HttpGetAsync(query);
    }

    private async Task<string> HttpPostAsync(string content)
    {
        using var response = await _client.PostAsync(_registerUrl, new StringContent(content, Encoding.UTF8, "application/json"));
        var responseString = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode) return responseString;

        throw new HttpRequestException(string.IsNullOrWhiteSpace(responseString) ? 
            $"Response status code does not indicate success: {response.StatusCode}" : responseString);
    }

    private async Task<string> HttpGetAsync(string query)
    {
        using var response = await _client.GetAsync(_queryUrl + query);
        var responseString = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode) return responseString;

        throw new HttpRequestException(string.IsNullOrWhiteSpace(responseString) ?
            $"Response status code does not indicate success: {response.StatusCode}" : responseString);
    }
}