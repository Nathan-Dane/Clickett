using Clickett.Models;
using Clickett.Native;
using Clickett.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Clickett.Services
{
    public sealed class ClickService : IClickService
    {
        private CancellationTokenSource? _cts;
        private readonly List<Task> _rocketTasks = new();
        private long _clickCount;

        public bool IsClicking { get; private set; }

        public event EventHandler<long>? ClickCountChanged;
        public event EventHandler? BurstCompleted;

        private readonly DispatcherTimer _cursorLockTimer;
        private ClickProfile? _cursorLockProfile;

        public ClickService()
        {
            _cursorLockTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };

            _cursorLockTimer.Tick += (_, _) =>
            {
                if (_cursorLockProfile is null)
                    return;

                if (!_cursorLockProfile.LockToLocation)
                    return;

                NativeMethods.SetCursorPos(
                    (int)_cursorLockProfile.XPosition,
                    (int)_cursorLockProfile.YPosition);
            };
        }


        public async Task StartAsync(ClickProfile profile, CancellationToken cancellationToken)
        {
            if (IsClicking) return;

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            IsClicking = true;
            _clickCount = 0;

            try
            {
                DoClick(profile);

                if (profile.IsBurstMode && _clickCount >= profile.BurstCount)
                {
                    BurstCompleted?.Invoke(this, EventArgs.Empty);
                    Stop();
                    return;
                }

                if (profile.RocketMode)
                    await RunRocketModeAsync(profile, _cts.Token);
                else
                    await RunNormalModeAsync(profile, _cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                IsClicking = false;
                _rocketTasks.Clear();
            }
        }

        public void Stop()
        {
            StopCursorLock();
            _cts?.Cancel();
        }

        private async Task RunNormalModeAsync(ClickProfile profile, CancellationToken token)
        {
            int burstProgress = 0;
            int interval = Math.Max(1, profile.ClickInterval);
            bool capped = profile.IsBurstMode;

            while (!token.IsCancellationRequested)
            {
                if (profile.Jitter)
                    await ApplyJitterAsync(interval, token);

                DoClick(profile);

                burstProgress += profile.DoubleClick ? 2 : 1;

                if (capped && burstProgress >= profile.BurstCount)
                {
                    BurstCompleted?.Invoke(this, EventArgs.Empty);
                    Stop();
                    return;
                }

                await Task.Delay(interval, token);
            }
        }

        private async Task RunRocketModeAsync(ClickProfile profile, CancellationToken token)
        {
            int threadCount = Math.Max(1, profile.Threads);

            for (int i = 0; i < threadCount; i++)
            {
                int workerIndex = i;

                _rocketTasks.Add(Task.Run(() =>
                {
                    RunRocketWorker(profile, workerIndex, threadCount, token);
                }, token));

                await Task.Delay(1, token);
            }

            await Task.WhenAll(_rocketTasks);
        }

        private void RunRocketWorker(
            ClickProfile profile,
            int workerIndex,
            int threadCount,
            CancellationToken token)
        {
            int localClicks = 0;
            bool capped = profile.IsBurstMode;
            int workerBurstLimit = capped
                ? (int)Math.Ceiling((float)profile.BurstCount / threadCount)
                : int.MaxValue;

            while (!token.IsCancellationRequested)
            {
                if (capped && localClicks >= workerBurstLimit)
                {
                    BurstCompleted?.Invoke(this, EventArgs.Empty);
                    Stop();
                    return;
                }

                DoClick(profile);

                localClicks += profile.DoubleClick ? 2 : 1;

                Thread.Sleep(1);
            }
        }

        private async Task ApplyJitterAsync(int interval, CancellationToken token)
        {
            int jitterDelay = (int)Math.Floor(interval * Random.Shared.Next(0, 6) / 10f);

            if (jitterDelay > 0)
                await Task.Delay(jitterDelay, token);
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

            RegisterClick();

            if (profile.DoubleClick)
            {
                NativeMethods.mouse_event(
                    profile.ClickDownFlag | profile.ClickUpFlag,
                    profile.XPosition,
                    profile.YPosition,
                    0,
                    0);

                RegisterClick();
            }
        }

        private void RegisterClick()
        {
            long count = Interlocked.Increment(ref _clickCount);
            ClickCountChanged?.Invoke(this, count);
        }

        public void StartCursorLock(ClickProfile profile)
        {
            _cursorLockProfile = profile;

            if (profile.LockToLocation)
                _cursorLockTimer.Start();
        }

        public void StopCursorLock()
        {
            _cursorLockTimer.Stop();
            _cursorLockProfile = null;
        }

    }
}
