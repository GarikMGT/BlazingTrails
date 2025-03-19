using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrails;

public class AddTrailEndpoint(BlazingTrailsContext database) : EndpointBaseAsync
    .WithRequest<AddTrailRequest>
    .WithResult<ActionResult<int>>
{
    private readonly BlazingTrailsContext database = database;

    [Authorize]
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
            Owner = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "name")!.Value!,
            Waypoints = request.Trail.Waypoints.Select(w => new Waypoint
            {
                Latitude = w.Latitude,
                Longitude = w.Longitude,
            }).ToList(),
        };

        await database.Trails.AddAsync(trail, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Id);
    }
}
/* 
 {"id_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Il9zbDJJM1NzQjk4STZfY00wSWdibiJ9.eyJuaWNrbmFtZSI6ImEiLCJuYW1lIjoiYUBhLmNvbSIsInBpY3R1cmUiOiJodHRwczovL3MuZ3JhdmF0YXIuY29tL2F2YXRhci9kMTBjYThkMTEzMDFjMmY0OTkzYWMyMjc5Y2U0YjkzMD9zPTQ4MCZyPXBnJmQ9aHR0cHMlM0ElMkYlMkZjZG4uYXV0aDAuY29tJTJGYXZhdGFycyUyRmEucG5nIiwidXBkYXRlZF9hdCI6IjIwMjQtMTEtMDRUMTc6MDI6MzAuNDUyWiIsImVtYWlsIjoiYUBhLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwiaXNzIjoiaHR0cHM6Ly9kZXYtOGIyc3YxM2M0NHhwcmh3dy51cy5hdXRoMC5jb20vIiwiYXVkIjoiaTBja3J1WFZtNEdQV0syazB0WWlvek1PZ01mNm5WcWsiLCJpYXQiOjE3MzA3NDA3NzksImV4cCI6MTczMDc3Njc3OSwic3ViIjoiYXV0aDB8NjcxYmQzZjkyOTZiNDM0MTc2ZjljYzhiIiwic2lkIjoiN1BueUNLM1ZadTk0N2JLZUtra0sxNFlhVjJScnpUSHoifQ.zYf9EPKW0Yzmt3RSGjdPwl1IXTpCWfnCNQz6fctmrPiI-AGdV-duWc5SAvj8-I2Ur2D0CTVLGQjZB7ZkOABmcXol-XLHXUYQfku0o_BZQ2nGF1BiZEp9Mc8di2dMXsx5R9HAc1JrePeuEnfaXGXWgYKkTS08bkUuP3QLS5aV_xIFNDcV6wUG1UGx9be6-g44LJMZlW8vQYVUVVvT0CQid1k-KjcareWIGjoWscJIReqbMoaN2-Vilu_WTCZNr-PBHbCDsFSzoyuORyL0xncqEl9Uf2pEPYxMtpwvmEIqqgxTq15Pvxpk1mQ3wFhxxDa7f59EDtuZbFB78OAQmo0Nqw","access_token":"eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6Il9zbDJJM1NzQjk4STZfY00wSWdibiJ9.eyJpc3MiOiJodHRwczovL2Rldi04YjJzdjEzYzQ0eHByaHd3LnVzLmF1dGgwLmNvbS8iLCJzdWIiOiJhdXRoMHw2NzFiZDNmOTI5NmI0MzQxNzZmOWNjOGIiLCJhdWQiOlsiaHR0cHM6Ly9ibGF6aW5ndHJhaWxzYXBpLmNvbSIsImh0dHBzOi8vZGV2LThiMnN2MTNjNDR4cHJod3cudXMuYXV0aDAuY29tL3VzZXJpbmZvIl0sImlhdCI6MTczMDc0MDc3OSwiZXhwIjoxNzMwODI3MTc5LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIGVtYWlsIiwiYXpwIjoiaTBja3J1WFZtNEdQV0syazB0WWlvek1PZ01mNm5WcWsifQ.HD3WQ-ArzA7G6KJWbpV_vWS0B13RqJP38PjLNStZ-Q5qZ_zjWoobe7RGBhnSaFVZveESIY2YFRkPU9F6L2PqjxlqF48ALR6lBTuCaw8SWMJI37nrQeSxGGVBShZfgHyTkikKJuKXqmixXkAaSRgM-e2i7gCNYCU_iHKDhhCfgMjxNsPv6MZt2NJHbNydyYjt-5MfcgQ4FAd_8VdgyIg8iYmSCN--xVBZkn6uNN0DFJjpG1D_c2I14pk_spgsWZSMMmtrtBVemu_N9UnVqojSHEjE-dKBm3stUwlvtZsaf-_V93a9fNg_tzeyQppleA0yqAnGY_4IdisMkaTmF245yw","token_type":"Bearer","scope":"openid profile email","profile":{"nickname":"a","name":"a@a.com","picture":"https://s.gravatar.com/avatar/d10ca8d11301c2f4993ac2279ce4b930?s=480&r=pg&d=https%3A%2F%2Fcdn.auth0.com%2Favatars%2Fa.png","updated_at":"2024-11-04T17:02:30.452Z","email":"a@a.com","email_verified":false,"sub":"auth0|671bd3f9296b434176f9cc8b","sid":"7PnyCK3VZu947bKeKkkK14YaV2RrzTHz"},"expires_at":1730827179}
*/