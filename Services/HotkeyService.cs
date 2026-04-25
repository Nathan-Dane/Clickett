using System;
using System.Text;
using System.Windows.Input;
using Clickett.Native;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class HotkeyService : IHotkeyService
    {
        public bool IsRegistered { get; private set; }

        public void Register(IntPtr windowHandle, int hotkeyId, Key key, bool ctrl, bool shift, bool alt)
        {
            if (key == Key.None)
                throw new InvalidOperationException("Cannot register an empty hotkey.");

            int modifiers = BuildModifiers(ctrl, shift, alt, includeNoRepeat: true);
            int virtualKey = KeyInterop.VirtualKeyFromKey(key);

            bool success = NativeMethods.RegisterHotKey(windowHandle, hotkeyId, modifiers, virtualKey);

            if (!success)
                throw new InvalidOperationException("Could not register global hotkey.");

            IsRegistered = true;
        }

        public void Unregister(IntPtr windowHandle, int hotkeyId)
        {
            NativeMethods.UnregisterHotKey(windowHandle, hotkeyId);
            IsRegistered = false;
        }

        public bool IsHotkeyMessage(IntPtr wParam, int hotkeyId)
        {
            return wParam.ToInt64() == hotkeyId;
        }

        public string FormatHotkey(Key key, bool ctrl, bool shift, bool alt)
        {
            return (ctrl ? "Ctrl + " : "")
                 + (shift ? "Shift + " : "")
                 + (alt ? "Alt + " : "")
                 + FormatKey(key);
        }

        public string FormatKey(Key key)
        {
            var buffer = new StringBuilder(256);
            var keyboardState = new byte[256];

            NativeMethods.ToUnicode(
                (uint)KeyInterop.VirtualKeyFromKey(key),
                0,
                keyboardState,
                buffer,
                256,
                0);

            string result = buffer.ToString();

            if (string.IsNullOrWhiteSpace(result))
                result = key.ToString();

            return result[0].ToString().ToUpper() + result[1..].ToLower();
        }

        private static int BuildModifiers(bool ctrl, bool shift, bool alt, bool includeNoRepeat)
        {
            int modifiers = 0;

            if (alt) modifiers |= NativeConstants.ModAlt;
            if (ctrl) modifiers |= NativeConstants.ModControl;
            if (shift) modifiers |= NativeConstants.ModShift;
            if (includeNoRepeat) modifiers |= NativeConstants.ModNoRepeat;

            return modifiers;
        }
    }
}
