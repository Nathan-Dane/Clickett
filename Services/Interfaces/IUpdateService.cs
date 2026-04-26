using System;
using System.Threading.Tasks;

namespace Clickett.Services.Interfaces
{
    public interface IUpdateService
    {
        bool HasUpdate { get; }

        event EventHandler? UpdateAvailable;
        event EventHandler<int>? DownloadProgressChanged;
        event EventHandler<string>? UpdateCheckFailed;
        event EventHandler<string>? UpdateDownloadFailed;

        Task CheckAndDownloadUpdateAsync(bool manual);
        void ApplyUpdateAndRestart();
    }
}
