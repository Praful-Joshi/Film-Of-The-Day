using System.Security.Claims;
using FilmOfTheDay.Web.Models.Connection;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IConnectionService
{
    public Task SendRequestAsync(int senderId, int receiverId);
    public Task AcceptRequestAsync(int senderId, int receiverId);
    public Task RemoveFriendAsync(int userId, int friendId);
    public FriendshipState GetFriendshipState(int userId, int profileUserId);
    public int GetFriendsCount(int userId);
    public Task<FriendsPageViewModel> GetFriendsPageViewModelAsync(ClaimsPrincipal currentUser);
    public Task<List<int>> GetAllFriendsIdsAsync(int userId);
}

public enum FriendshipState
{
    None,
    RequestSent,
    RequestReceived,
    Friends
}
