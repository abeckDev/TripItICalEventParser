using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AbeckDev.ICalEventParser.Models;
using System.Globalization;

namespace AbeckDev.ICalEventParser;

/// <summary>
/// Azure Function that parses iCal feeds and converts events to JSON format.
/// Specifically designed for TripIt integration with Microsoft Power Automate.
/// </summary>
public class ParseICalFeed
{
    private readonly ILogger<ParseICalFeed> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParseICalFeed"/> class.
    /// </summary>
    /// <param name="logger">The logger instance for this function.</param>
    public ParseICalFeed(ILogger<ParseICalFeed> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// HTTP-triggered Azure Function that downloads and parses an iCal feed,
    /// returning events from the current calendar year as JSON.
    /// </summary>
    /// <param name="req">The HTTP request containing the iCal feed URL.</param>
    /// <returns>
    /// A JSON array of <see cref="TripEvent"/> objects representing the parsed events,
    /// or a BadRequest response if the URL is invalid or the feed cannot be downloaded.
    /// </returns>
    /// <remarks>
    /// This function:
    /// - Downloads the iCal feed from the provided URL
    /// - Filters events to the current calendar year (starting January 1st)
    /// - Converts event times to UTC
    /// - Returns only basic event information suitable for Power Automate processing
    /// 
    /// Example usage:
    /// GET /api/ParseICalFeed?icalFeedUrl=https://www.tripit.com/feed/ical/private/[key]/tripit.ics
    /// </remarks>
    [Function("ParseICalFeed")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Extract and validate the iCal feed URL from query parameters
        string icalFeedUrl = req.Query["icalFeedUrl"];
        string travelerName = req.Query["travelerName"];
        if (string.IsNullOrEmpty(icalFeedUrl) || string.IsNullOrEmpty(travelerName))
        {
            return new BadRequestObjectResult("Missing 'icalFeedUrl' and/or 'travelerName' query parameter.");
        }

        // Download the iCal feed from the provided URL
        using var httpClient = new HttpClient();
        HttpResponseMessage response;
        try
        {
            response = await httpClient.GetAsync(icalFeedUrl);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download iCal feed.");
            return new BadRequestObjectResult("Failed to download iCal feed.");
        }

        // Parse the iCal content using iCal.NET library
        var calendar = Ical.Net.Calendar.Load(await response.Content.ReadAsStringAsync());

        // Define the start date for event filtering (beginning of current year in Berlin timezone)
        // This ensures we only get events from the current calendar year
        var Start = new CalDateTime(new DateTime(DateTime.UtcNow.Year, 1, 1), "Europe/Berlin");

        // Get all event occurrences from the start date onwards and remove duplicates
        var events = calendar.GetOccurrences(Start)
        .Select(o => o.Source)
        .Cast<CalendarEvent>()
        .Distinct()
        .ToList();

        // Convert calendar events to TripEvent objects for JSON serialization
        var responseEvents = new List<TripEvent>();
        foreach (var calendarEvent in events)
        {
            responseEvents.Add(new TripEvent
            {
                EventTitel = calendarEvent.Summary,
                Location = calendarEvent.Location,
                Description = calendarEvent.Description,
                EventStart = calendarEvent.Start.AsUtc,
                EventEnd = calendarEvent.End.AsUtc,
                IsTripHeadEvent = calendarEvent.Description != null && calendarEvent.Description.StartsWith($"{travelerName} is in")
            });
        }

        return new OkObjectResult(responseEvents);

    }
}
