using System;
using Clickett.Models;

namespace Clickett.Services.Interfaces
{
    public interface IAutoClickerController
    {
        bool IsActive { get; }
        bool IsClicking { get; }

        event EventHandler? Activated;
        event EventHandler? Deactivated;
        event EventHandler? ClickingStarted;
        event EventHandler? ClickingStopped;
        event EventHandler<long>? ClickCountChanged;

        void UpdateProfile(ClickProfile profile);
        void ToggleActive();
        void Activate();
        void Deactivate();
        void HandleHotkeyPressed();
        void StopClicking();
    }
}