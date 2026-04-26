using System;
using System.Windows.Input;
using System.Windows.Threading;
using Clickett.Models;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class AutoClickerController : IAutoClickerController
    {
        private readonly IClickSessionController _clickSessionController;
        private readonly DispatcherTimer _holdTimer;

        private ClickProfile _profile = new();

        public bool IsActive { get; private set; }
        public bool IsClicking { get; private set; }

        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler? ClickingStarted;
        public event EventHandler? ClickingStopped;
        public event EventHandler<long>? ClickCountChanged;

        public AutoClickerController(IClickSessionController clickSessionController)
        {
            _clickSessionController = clickSessionController;

            _clickSessionController.ClickCountChanged += (_, count) =>
                ClickCountChanged?.Invoke(this, count);

            _clickSessionController.SessionEnded += (_, _) =>
                StopClicking();

            _holdTimer = new DispatcherTimer(DispatcherPriority.Send)
            {
                Interval = TimeSpan.FromMilliseconds(1)
            };

            _holdTimer.Tick += HoldCheck;
        }

        public void UpdateProfile(ClickProfile profile)
        {
            _profile = profile;
        }

        public void ToggleActive()
        {
            if (IsActive) Deactivate();
            else Activate();
        }

        public void Activate()
        {
            if (IsActive) return;

            IsActive = true;
            IsClicking = false;

            Activated?.Invoke(this, EventArgs.Empty);
        }

        public void Deactivate()
        {
            if (!IsActive) return;

            StopClicking();

            IsActive = false;

            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void HandleHotkeyPressed()
        {
            if (!IsActive) return;

            switch (_profile.Mode)
            {
                case ClickMode.Burst:
                    if (IsClicking)
                        StopClicking();
                    else
                        StartClicking();
                    break;

                case ClickMode.Toggle:
                    if (IsClicking)
                        StopClicking();
                    else
                        StartClicking();
                    break;

                case ClickMode.Hold:
                    if (IsClicking) return;

                    StartClicking();
                    _holdTimer.Start();
                    break;
            }
        }

        public void StopClicking()
        {
            if (!IsClicking) return;

            _holdTimer.Stop();
            _clickSessionController.Stop();

            IsClicking = false;

            ClickingStopped?.Invoke(this, EventArgs.Empty);
        }

        private void StartClicking()
        {
            if (IsClicking) return;

            IsClicking = true;

            ClickingStarted?.Invoke(this, EventArgs.Empty);

            _ = _clickSessionController.StartAsync(_profile);
        }

        private void HoldCheck(object? sender, EventArgs e)
        {
            if (_profile.Mode != ClickMode.Hold)
                return;

            if (Keyboard.IsKeyUp(_profile.Hotkey))
            {
                StopClicking();
            }
        }
    }
}
