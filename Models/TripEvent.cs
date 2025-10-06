using System;

namespace AbeckDev.ICalEventParser.Models;

/// <summary>
/// Represents a simplified travel/trip event parsed from an iCal feed.
/// Contains basic event information suitable for Power Automate integration.
/// </summary>
public class TripEvent
{
    /// <summary>
    /// Gets or sets the title/summary of the event.
    /// </summary>
    /// <value>The event title from the iCal SUMMARY field.</value>
    public string EventTitel { get; set; }

    /// <summary>
    /// Gets or sets the location where the event takes place.
    /// </summary>
    /// <value>The event location from the iCal LOCATION field.</value>
    public string Location { get; set; }

    /// <summary>
    /// Gets or sets the start date and time of the event in UTC.
    /// </summary>
    /// <value>The event start time converted to UTC for consistent processing.</value>
    public DateTime EventStart { get; set; }

    /// <summary>
    /// Gets or sets the end date and time of the event in UTC.
    /// </summary>
    /// <value>The event end time converted to UTC for consistent processing.</value>
    public DateTime EventEnd { get; set; }

    /// <summary>
    /// Gets or sets the detailed description of the event.
    /// </summary>
    /// <value>The event description from the iCal DESCRIPTION field.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the timezone of the event.
    /// </summary>
    /// <value>The event timezone identifier (e.g., "Europe/Berlin").</value>
    public string Timezone { get; set; }

    /// <summary>
    /// Gets or sets whether this event represents a trip header (parent trip container) or an individual trip event.
    /// </summary>
    /// <value>
    /// <c>true</c> if this is a trip header event (main trip container in TripIt); 
    /// <c>false</c> if this is an individual event within a trip (flight, hotel, etc.).
    /// </value>
    /// <remarks>
    /// Trip headers are identified by their description starting with "[TravelerName] is in [Location]".
    /// While all events are returned in UTC format for consistent processing, this flag allows downstream
    /// consumers (like Logic Apps or Power Automate) to apply different timezone handling if needed:
    /// - Trip header events: May be displayed in European timezone for better user experience
    /// - Individual events: Typically kept in UTC or converted to local event timezone
    /// 
    /// This distinction is important for proper calendar display, as trip headers represent
    /// the overall trip period, while individual events have specific local times.
    /// </remarks>
    public bool IsTripHeadEvent { get; set; }

}
