using Clickett.Services.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Clickett.Services
{
    public sealed class NotificationService : INotificationService
    {
        public void Show(string title, string? message = null)
        {
            ToastNotificationManagerCompat.History.Clear();

            var builder = new ToastContentBuilder()
                .AddText(title);

            if (!string.IsNullOrWhiteSpace(message))
            {
                builder.AddText(message);
            }

            builder.Show();
        }
    }
}
