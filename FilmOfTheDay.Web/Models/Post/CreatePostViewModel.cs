using System.ComponentModel.DataAnnotations;

namespace FilmOfTheDay.Web.Models.Post;
public class CreatePostViewModel
{
    public string SearchQuery { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string? SelectedMovieTitle { get; set; }

    public string? SelectedMoviePosterUrl { get; set; }

    public string? SelectedMovieLink { get; set; }

    public List<MovieResult>? SearchResults { get; set; }
}

public class MovieResult
{
    public string Title { get; set; } = string.Empty;
    public string PosterUrl { get; set; } = string.Empty;
    public string MovieUrl { get; set; } = string.Empty;
}
