namespace Clickett.Services.Interfaces
{
    public interface INotificationService
    {
        void Show(string title, string? message = null);
    }
}
