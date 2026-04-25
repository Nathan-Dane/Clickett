using System;
using System.Threading;
using System.Threading.Tasks;
using Clickett.Models;
using Clickett.Native;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class ClickService : IClickService
    {
        private CancellationTokenSource? _cts;
        private long _clickCount;

        public bool IsClicking { get; private set; }

        public event EventHandler<long>? ClickCountChanged;
        public event EventHandler? BurstCompleted;

        public async Task StartAsync(ClickProfile profile, CancellationToken cancellationToken)
        {
            if (IsClicking) return;

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            IsClicking = true;
            _clickCount = 0;

            try
            {
                await RunClickLoopAsync(profile, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Expected when stopping.
            }
            finally
            {
                IsClicking = false;
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
        }

        private async Task RunClickLoopAsync(ClickProfile profile, CancellationToken token)
        {
            int burstProgress = 0;
            int interval = Math.Max(1, profile.ClickInterval);
            bool capped = profile.IsBurstMode;

            while (!token.IsCancellationRequested)
            {
                DoClick(profile);

                burstProgress++;

                if (capped && burstProgress >= profile.BurstCount)
                {
                    BurstCompleted?.Invoke(this, EventArgs.Empty);
                    Stop();
                    return;
                }

                await Task.Delay(interval, token);
            }
        }

        private void DoClick(ClickProfile profile)
        {
            if (profile.LockToLocation)
            {
                NativeMethods.SetCursorPos((int)profile.XPosition, (int)profile.YPosition);
            }

            NativeMethods.mouse_event(
                profile.ClickDownFlag | profile.ClickUpFlag,
                profile.XPosition,
                profile.YPosition,
                0,
                0);

            _clickCount++;
            ClickCountChanged?.Invoke(this, _clickCount);

            if (profile.DoubleClick)
            {
                NativeMethods.mouse_event(
                    profile.ClickDownFlag | profile.ClickUpFlag,
                    profile.XPosition,
                    profile.YPosition,
                    0,
                    0);

                _clickCount++;
                ClickCountChanged?.Invoke(this, _clickCount);
            }
        }
    }
}
