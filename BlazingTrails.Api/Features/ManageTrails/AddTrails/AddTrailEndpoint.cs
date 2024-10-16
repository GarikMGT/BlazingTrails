﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrails;

public class AddTrailEndpoint(BlazingTrailsContext database) : EndpointBaseAsync
    .WithRequest<AddTrailRequest>
    .WithResult<ActionResult<int>>
{
    private readonly BlazingTrailsContext database = database;

    [HttpPost(AddTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<int>> HandleAsync(AddTrailRequest request, CancellationToken cancellationToken = default)
    {
        var trail = new Trail
        {
            Name = request.Trail.Name,
            Description = request.Trail.Description,
            Location = request.Trail.Location,
            TimeInMinutes = request.Trail.TimeInMinutes,
            Length = request.Trail.Length,
            Waypoints = request.Trail.Waypoints.Select(w => new Waypoint{
                Latitude = w.Latitude,
                Longitude = w.Longitude,
            }).ToList(),
        };

        await database.Trails.AddAsync(trail, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Id);
    }
}
