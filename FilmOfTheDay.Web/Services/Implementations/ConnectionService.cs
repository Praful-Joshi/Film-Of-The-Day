using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Services.Interfaces;

namespace FilmOfTheDay.Web.Services.Implementations;

public class ConnectionService : IConnectionService
{
    private readonly ApplicationDbContext _dbContext;

    public ConnectionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SendRequest(int senderId, int receiverId)
    {
        if (senderId == receiverId)
            throw new InvalidOperationException("You cannot send a request to yourself.");
        if (AreFriends(senderId, receiverId))
            throw new InvalidOperationException("Friendship already exists.");
        else if (HasPendingRequest(senderId, receiverId))
            throw new InvalidOperationException("A pending request already exists.");

        var friendship = new Friendship
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = FriendshipStatus.Pending
        };

        _dbContext.Friendships.Add(friendship);
        _dbContext.SaveChanges();
    }
    public bool AreFriends(int userId1, int userId2)
    {
        return _dbContext.Friendships.Any(f =>
            ((f.SenderId == userId1 && f.ReceiverId == userId2) ||
                (f.SenderId == userId2 && f.ReceiverId == userId1)) &&
                f.Status == FriendshipStatus.Accepted);
    }

    public bool HasPendingRequest(int senderId, int receiverId)
    {
        return _dbContext.Friendships.Any(f =>
            f.SenderId == senderId && f.ReceiverId == receiverId && f.Status == FriendshipStatus.Pending);
    }

    public void AcceptRequest(int senderId, int receiverId)
    {
        var request = _dbContext.Friendships
            .FirstOrDefault(f => f.SenderId == senderId && f.ReceiverId == receiverId && f.Status == FriendshipStatus.Pending);

        if (request == null)
            throw new InvalidOperationException("Request not found.");

        request.Status = FriendshipStatus.Accepted;
        _dbContext.SaveChanges();
    }

    public FriendshipState GetFriendshipState(int userId, int profileUserId)
    {
        var friendship = _dbContext.Friendships
            .FirstOrDefault(f =>
                (f.SenderId == userId && f.ReceiverId == profileUserId) ||
                (f.SenderId == profileUserId && f.ReceiverId == userId));

        if (friendship == null)
            return FriendshipState.None;

        return friendship.Status switch
        {
            FriendshipStatus.Pending => FriendshipState.Pending,
            FriendshipStatus.Accepted => FriendshipState.Friends,
            _ => FriendshipState.None
        };
    }
}

public enum FriendshipState
{
    None,
    Pending,
    Friends
}

