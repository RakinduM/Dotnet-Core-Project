using Dotnet_Core_Project.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGameById";

List<GameDto> games = [
    new (1, "GTA V", "Action", 29.99m, new DateOnly(2013, 9, 17)),
    new (2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new (3, "The Witcher 3", "RPG", 29.99m, new DateOnly(2015, 5, 19))
];

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (int id) => games.Find(g => g.Id == id)).WithName(GetGameEndpointName);

app.MapPost("/games", (CreateGameDto newGame) =>
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

app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) => {

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

app.MapDelete("/games/{id}", (int id) => {

    var index = games.FindIndex(g => g.Id == id);

    if (index == -1)
    {
        return Results.NotFound();
    }

    games.RemoveAt(index);

    return Results.NoContent();
});

app.Run();
