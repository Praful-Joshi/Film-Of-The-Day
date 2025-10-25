namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IConnectionService
{
    public Task SendRequestAsync(int senderId, int receiverId);
    public Task AcceptRequestAsync(int senderId, int receiverId);
    public FriendshipState GetFriendshipState(int userId, int profileUserId);
    public int GetFriendsCount(int userId);
}

public enum FriendshipState
{
    None,
    RequestSent,
    RequestReceived,
    Friends
}
