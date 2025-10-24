using FilmOfTheDay.Web.Models.Auth;
using System.Security.Claims;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IAuthService
{
    public Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterViewModel model);
    public Task<(bool Success, string? ErrorMessage, ClaimsPrincipal? Principal)> LoginAsync(LoginViewModel model);
    public Task LogoutAsync();
    public bool IsAuthenticated(ClaimsPrincipal user);
}
