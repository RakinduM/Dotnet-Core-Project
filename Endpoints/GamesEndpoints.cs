using System;
using Dotnet_Core_Project.Dtos;

namespace Dotnet_Core_Project.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGameById";
    private static readonly List<GameDto> games = [
    new (1, "GTA V", "Action", 29.99m, new DateOnly(2013, 9, 17)),
    new (2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new (3, "The Witcher 3", "RPG", 29.99m, new DateOnly(2015, 5, 19))
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet("/", () => games);

        group.MapGet("/{id}", (int id) =>
        {
            GameDto? game = games.Find(g => g.Id == id);

            return game is not null ? Results.Ok(game) : Results.NotFound();
        }).WithName(GetGameEndpointName);

        group.MapPost("/", (CreateGameDto newGame) =>
        {

            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
        });

        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {

            var index = games.FindIndex(g => g.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games[index] = new GameDto
            (
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            return Results.Ok(games[index]);
        });

        group.MapDelete("/{id}", (int id) =>
        {

            var index = games.FindIndex(g => g.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }

            games.RemoveAt(index);

            return Results.NoContent();
        });

        return group;
    }
}
