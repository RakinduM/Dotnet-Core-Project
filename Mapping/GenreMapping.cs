using System;
using Dotnet_Core_Project.Dtos;
using Dotnet_Core_Project.Entities;

namespace Dotnet_Core_Project.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }
}
