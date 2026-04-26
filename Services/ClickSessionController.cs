using System;
using System.Threading;
using System.Threading.Tasks;
using Clickett.Models;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class ClickSessionController : IClickSessionController
    {
        private readonly IClickService _clickService;
        private CancellationTokenSource? _cts;

        public bool IsRunning { get; private set; }

        public event EventHandler<long>? ClickCountChanged;
        public event EventHandler? SessionEnded;

        public ClickSessionController(IClickService clickService)
        {
            _clickService = clickService;

            _clickService.ClickCountChanged += (_, count) =>
                ClickCountChanged?.Invoke(this, count);

            _clickService.BurstCompleted += (_, _) =>
                Stop();
        }

        public async Task StartAsync(ClickProfile profile)
        {
            if (IsRunning) return;

            _cts = new CancellationTokenSource();
            IsRunning = true;

            try
            {
                _clickService.StartCursorLock(profile);
                await _clickService.StartAsync(profile, _cts.Token);
            }
            finally
            {
                _clickService.StopCursorLock();
                IsRunning = false;
                SessionEnded?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            _clickService.Stop();
        }
    }
}
