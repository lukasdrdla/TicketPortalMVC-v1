﻿using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Application.Services.Interface;

public interface IEventService
{
    Task<List<Event>> GetEventsAsync();
    Task<Event> GetEventByIdAsync(int id);
    Task CreateEventAsync(Event @event);
    Task UpdateEventAsync(Event @event);
    Task DeleteEventAsync(int id);

    // Extra methods
    Task<int> GetTotalEventsAsync();
    Task<List<Event>> SearchEventsAsync(string term);

}