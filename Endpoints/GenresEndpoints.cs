using System;
using System.Text.RegularExpressions;
using Dotnet_Core_Project.Data;
using Dotnet_Core_Project.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Core_Project.Endpoints;

public static class GenresEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");
        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            await dbContext.Genres
                .Select(genre => genre.ToDto())
                .AsNoTracking()
                .ToListAsync();
        });
        return group;
    }
}
