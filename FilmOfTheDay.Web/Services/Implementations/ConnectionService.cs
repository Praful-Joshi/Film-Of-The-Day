using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Services.Interfaces;

namespace FilmOfTheDay.Web.Services.Implementations;
public class ConnectionService : IConnectionService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly INotificationService _notifService;

    public ConnectionService(ApplicationDbContext dbContext, INotificationService notifService)
    {
        _dbContext = dbContext;
        _notifService = notifService;
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
        _notifService.CreateNotificationAsync(receiverId, NotificationType.NewFollower, "You have a new friend request.", null).Wait();
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
        // Check if the current user sent a request to the profile user
        var sent = _dbContext.Friendships
            .FirstOrDefault(f => f.SenderId == userId && f.ReceiverId == profileUserId);

        if (sent != null)
        {
            return sent.Status switch
            {
                FriendshipStatus.Pending => FriendshipState.RequestSent,
                FriendshipStatus.Accepted => FriendshipState.Friends,
                _ => FriendshipState.None
            };
        }

        // Check if the profile user sent a request to the current user
        var received = _dbContext.Friendships
            .FirstOrDefault(f => f.SenderId == profileUserId && f.ReceiverId == userId);

        if (received != null)
        {
            return received.Status switch
            {
                FriendshipStatus.Pending => FriendshipState.RequestReceived,
                FriendshipStatus.Accepted => FriendshipState.Friends,
                _ => FriendshipState.None
            };
        }

        return FriendshipState.None;
    }

    public int GetFriendsCount(int userId)
    {
        return _dbContext.Friendships.Count(f =>
            (f.SenderId == userId || f.ReceiverId == userId) &&
            f.Status == FriendshipStatus.Accepted);
    }
}

public enum FriendshipState
{
    None,
    RequestSent,
    RequestReceived,
    Friends
}

