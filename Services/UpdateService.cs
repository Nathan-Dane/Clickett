using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Clickett.Services.Interfaces;
using Velopack;
using Velopack.Sources;

namespace Clickett.Services
{
    public sealed class UpdateService : IUpdateService
    {
        private readonly UpdateManager _updateManager;
        private UpdateInfo? _update;

        public bool HasUpdate => _update is not null;

        public event EventHandler? UpdateAvailable;
        public event EventHandler<int>? DownloadProgressChanged;
        public event EventHandler<string>? UpdateCheckFailed;
        public event EventHandler<string>? UpdateDownloadFailed;

        public UpdateService()
        {
            _updateManager = new UpdateManager(
                new GithubSource("https://github.com/Nathan-Dane/Clickett", null, false));
        }

        public async Task CheckAndDownloadUpdateAsync(bool manual)
        {
            try
            {
                _update = await _updateManager.CheckForUpdatesAsync()
                    .ConfigureAwait(false);
            }
            catch
            {
                if (manual)
                    UpdateCheckFailed?.Invoke(this, "Failed to check for updates");

                return;
            }


            if (_update is null)
                return;

            UpdateAvailable?.Invoke(this, EventArgs.Empty);

            try
            {
                await _updateManager.DownloadUpdatesAsync(
                    _update,
                    progress => DownloadProgressChanged?.Invoke(this, progress))
                    .ConfigureAwait(false);
            }
            catch
            {
                try
                {
                    await _updateManager.DownloadUpdatesAsync(
                        _update,
                        progress => DownloadProgressChanged?.Invoke(this, progress),
                        ignoreDeltas: true)
                        .ConfigureAwait(false);
                }
                catch
                {
                    if (manual)
                        UpdateDownloadFailed?.Invoke(this, "Failed to download update");
                }
            }
        }

        public void ApplyUpdateAndRestart()
        {
            if (_update is null)
                return;

            _updateManager.ApplyUpdatesAndRestart(_update);
        }
    }
}
