﻿namespace Events
{
    using System;
    using Wintellect.PowerCollections;

    /// <summary>
    /// Represents a collection of <see cref="Event"/> objects.
    /// </summary>
    public class EventHolder
    {
        private MultiDictionary<string, Event> byTitle = new MultiDictionary<string, Event>(true);
        private OrderedBag<Event> byDateAndTime = new OrderedBag<Event>();

        /// <summary>
        /// Creates an event and adds it in the event holder.
        /// An "Event added" message is also added to the message log.
        /// </summary>
        /// <param name="dateAndTime">The date and time of the event.</param>
        /// <param name="title">The title of the event.</param>
        /// <param name="location">The location of the event.</param>
        public void AddEvent(DateTime dateAndTime, string title, string location)
        {
            Event newEvent = new Event(dateAndTime, title, location);
            this.byTitle.Add(title.ToLower(), newEvent);
            this.byDateAndTime.Add(newEvent);
            Messages.EventAdded();
        }

        /// <summary>
        /// Deletes all events that have the specified title.
        /// Performs a case-insensitive search.
        /// A message telling the number of deleted events is added to the log.
        /// </summary>
        /// <param name="titleToDelete">The title to delete.</param>
        public void DeleteEvents(string titleToDelete)
        {
            string title = titleToDelete.ToLower();
            int removed = 0;
            foreach (var eventToRemove in this.byTitle[title])
            {
                removed++;
                this.byDateAndTime.Remove(eventToRemove);
            }

            this.byTitle.Remove(title);
            Messages.EventDeleted(removed);
        }

        /// <summary>
        /// Adds to the message log the string representation of all the messages
        /// which have the specified date and time.
        /// In case there are no events, the message "No events found" is added.
        /// </summary>
        /// <param name="dateAndTime">The date and time of the event.</param>
        /// <param name="count">The number of events to be added.</param>
        public void ListEvents(DateTime dateAndTime, int count)
        {
            OrderedBag<Event>.View eventsToShow = this.byDateAndTime.RangeFrom(new Event(dateAndTime, string.Empty, string.Empty), true);

            int shown = 0;
            foreach (var eventToShow in eventsToShow)
            {
                if (shown == count)
                {
                    break;
                }

                Messages.PrintEvent(eventToShow);
                shown++;
            }

            if (shown == 0)
            {
                Messages.NoEventsFound();
            }
        }
    }
}
