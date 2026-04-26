using System;

namespace Clickett.Services.Interfaces
{
    public interface ITrayIconService : IDisposable
    {
        bool IsVisible { get; }

        event EventHandler? ActivateRequested;
        event EventHandler? OpenRequested;
        event EventHandler? ExitRequested;

        void Show(bool isActive);
        void Hide();
        void SetActiveState(bool isActive);
    }
}
