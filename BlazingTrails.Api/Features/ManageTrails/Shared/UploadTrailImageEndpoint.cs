﻿using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Features.ManageTrails.Shared;

public class UploadTrailImageEndpoint(BlazingTrailsContext database) : EndpointBaseAsync
    .WithRequest<int>
    .WithResult<ActionResult<string>>
{
    private readonly BlazingTrailsContext database = database;

    [Authorize]
    [HttpPost(UploadTrailImageRequest.RouteTemplate)]
    public override async Task<ActionResult<string>> HandleAsync(
        [FromRoute] int trailId, CancellationToken cancellationToken = default)
    {
        var trail = await database.Trails
            .SingleOrDefaultAsync(x => x.Id == trailId, cancellationToken: cancellationToken);
        if (trail is null)
            return BadRequest("Trail does not exist.");

        if (!trail.Owner.Equals(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value, StringComparison.OrdinalIgnoreCase)
            && !HttpContext.User.IsInRole(Constants.Roles.Admin))
            return Unauthorized();

        var file = Request.Form.Files[0];
        if (file.Length == 0)
        {
            return BadRequest("No image found");
        }

        var filename = $"{Guid.NewGuid()}.jpg";
        var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Images", filename);

        var resizeOptions = new ResizeOptions
        {
            Mode = ResizeMode.Pad,
            Size = new Size(640, 426),
        };

        using var image = Image.Load(file.OpenReadStream());
        image.Mutate(x => x.Resize(resizeOptions));
        await image.SaveAsJpegAsync(saveLocation, cancellationToken);

        if (!string.IsNullOrWhiteSpace(trail.Image))
        {
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image));
        }

        trail.Image = filename;
        await database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Image);
    }
}
