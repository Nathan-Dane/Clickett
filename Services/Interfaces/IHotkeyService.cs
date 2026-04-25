using System;
using System.Windows.Input;

namespace Clickett.Services.Interfaces
{
    public interface IHotkeyService
    {
        event EventHandler? HotkeyPressed;

        bool IsRegistered { get; }

        void Register(IntPtr windowHandle, int hotkeyId, Key key, bool ctrl, bool shift, bool alt);
        void Unregister(IntPtr windowHandle, int hotkeyId);
        bool IsHotkeyMessage(IntPtr wParam, int hotkeyId);
        string FormatKey(Key key);
    }
}
