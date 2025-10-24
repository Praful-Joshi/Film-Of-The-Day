using FilmOfTheDay.Web.Services.Implementations;

namespace FilmOfTheDay.Web.Services.Interfaces;
public interface IConnectionService
{
    public void SendRequest(int senderId, int receiverId);
    public void AcceptRequest(int senderId, int receiverId);
    public FriendshipState GetFriendshipState(int userId, int profileUserId);
    public int GetFriendsCount(int userId);
}