using System;
using Dotnet_Core_Project.Data;
using Dotnet_Core_Project.Dtos;
using Dotnet_Core_Project.Entities;
using Dotnet_Core_Project.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Core_Project.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";
    private static readonly List<GameSummaryDto> games = [
    new (1, "GTA V", "Action", 29.99m, new DateOnly(2013, 9, 17)),
    new (2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new (3, "The Witcher 3", "RPG", 29.99m, new DateOnly(2015, 5, 19))
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games").WithParameterValidation();

        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games.Include(game => game.Genre).Select(game => game.ToGameSummaryDto()).AsNoTracking()
        );

        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);

            return game is not null ? Results.Ok(game.ToGameDetailsDto) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game.ToGameDetailsDto());
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {

            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {

           dbContext.Games.Where(game => game.Id == id).ExecuteDelete();
            return Results.NoContent();
        });

        return group;
    }
}
