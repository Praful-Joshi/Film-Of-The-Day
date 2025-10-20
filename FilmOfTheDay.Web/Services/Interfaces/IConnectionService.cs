namespace FilmOfTheDay.Web.Services.Interfaces;

public interface IConnectionService
{
    void SendRequest(int senderId, int receiverId);
}