using System.Diagnostics;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class ShellService : IShellService
    {
        public void OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo(url)
            {
                UseShellExecute = true
            });
        }

        public void OpenEmail(string mailtoUrl)
        {
            Process.Start(new ProcessStartInfo(mailtoUrl)
            {
                UseShellExecute = true
            });
        }
    }
}
