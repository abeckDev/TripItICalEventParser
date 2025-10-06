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
    /// <value>The event description from the iCal <-----> field.</value>
    public string Timezone { get; set; }

    /// <summary>
    /// Defines whether this is an iCal Event Describing the parent trip in TripIt (true) or an Event within a trip (false)
    /// </summary>
    public bool IsTripHeadEvent { get; set; }

}
