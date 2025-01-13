using System;
using Dotnet_Core_Project.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_Core_Project.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
}
