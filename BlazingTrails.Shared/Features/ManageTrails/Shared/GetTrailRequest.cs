﻿using MediatR;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared;

public record GetTrailRequest(int TrailId) : 
    IRequest<GetTrailRequest.Response>
{
    public const string RouteTemplate = "api/trails/{trailId}";

    public record Response(Trail Trail);
    public record Trail(int Id, string Name, string Location, string? Image,
        int TimeInMinutes, int Length, string Description, IEnumerable<RouteInstructions> RouteInstructions)
    public record RouteInstructions(int Id, int Stage, string Description);
}