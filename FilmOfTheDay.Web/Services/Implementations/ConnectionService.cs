using FilmOfTheDay.Core.Entities;
using FilmOfTheDay.Infrastructure.Data;
using FilmOfTheDay.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task SendRequestAsync(int senderId, int receiverId)
    {
        if (senderId == receiverId)
            throw new InvalidOperationException("You cannot send a request to yourself.");

        if (await AreFriendsAsync(senderId, receiverId))
            throw new InvalidOperationException("Friendship already exists.");

        if (await HasPendingRequestAsync(senderId, receiverId))
            throw new InvalidOperationException("A pending request already exists.");

        var friendship = new Friendship
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Status = FriendshipStatus.Pending
        };

        _dbContext.Friendships.Add(friendship);
        await _dbContext.SaveChangesAsync();

        await _notifService.CreateNotificationAsync(
            receiverId,
            NotificationType.NewFollower,
            "You have a new friend request.",
            null
        );
    }

    public async Task AcceptRequestAsync(int senderId, int receiverId)
    {
        var request = await _dbContext.Friendships
            .FirstOrDefaultAsync(f =>
                f.SenderId == senderId &&
                f.ReceiverId == receiverId &&
                f.Status == FriendshipStatus.Pending
            );

        if (request == null)
            throw new InvalidOperationException("Request not found.");

        request.Status = FriendshipStatus.Accepted;
        await _dbContext.SaveChangesAsync();
    }

    public FriendshipState GetFriendshipState(int userId, int profileUserId)
    {
        var friendship = _dbContext.Friendships.FirstOrDefault(f =>
            (f.SenderId == userId && f.ReceiverId == profileUserId) ||
            (f.SenderId == profileUserId && f.ReceiverId == userId));

        if (friendship == null)
            return FriendshipState.None;

        if (friendship.Status == FriendshipStatus.Accepted)
            return FriendshipState.Friends;

        if (friendship.SenderId == userId)
            return FriendshipState.RequestSent;

        if (friendship.ReceiverId == userId)
            return FriendshipState.RequestReceived;

        return FriendshipState.None;
    }

    public int GetFriendsCount(int userId)
    {
        return _dbContext.Friendships.Count(f =>
            (f.SenderId == userId || f.ReceiverId == userId) &&
            f.Status == FriendshipStatus.Accepted);
    }

    private async Task<bool> AreFriendsAsync(int userId1, int userId2)
    {
        return await _dbContext.Friendships.AnyAsync(f =>
            ((f.SenderId == userId1 && f.ReceiverId == userId2) ||
             (f.SenderId == userId2 && f.ReceiverId == userId1)) &&
            f.Status == FriendshipStatus.Accepted);
    }

    private async Task<bool> HasPendingRequestAsync(int senderId, int receiverId)
    {
        return await _dbContext.Friendships.AnyAsync(f =>
            f.SenderId == senderId &&
            f.ReceiverId == receiverId &&
            f.Status == FriendshipStatus.Pending);
    }
}
