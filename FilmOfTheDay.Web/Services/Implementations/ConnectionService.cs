using System.Diagnostics;
using System.Security.Claims;
using FilmOfTheDay.Web.Services.Interfaces;

namespace FilmOfTheDay.Web.Services.Implementations;
public class ConnectionService : IConnectionService
{
    public void SendRequest(int senderId, int receiverId)
    {
        // Logic to send a connection/friend request
        Debug.WriteLine($"Connection request sent from User {senderId} to User {receiverId}");
    }
}
