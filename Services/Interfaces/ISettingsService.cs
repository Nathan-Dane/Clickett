using System.Windows.Input;

namespace Clickett.Services.Interfaces
{
    public interface ISettingsService
    {
        Key Hotkey { get; set; }
        bool HotkeyCtrl { get; set; }
        bool HotkeyShift { get; set; }
        bool HotkeyAlt { get; set; }

        string Theme { get; set; }
        bool DoAnimations { get; set; }
        bool Jitter { get; set; }
        bool DoubleClick { get; set; }
        bool CountTotal { get; set; }
        bool AlwaysOnTop { get; set; }
        bool Startup { get; set; }
        bool TrayIcon { get; set; }
        bool MinimizeToTray { get; set; }

        int ClickInterval { get; set; }
        int BurstCount { get; set; }
        int UiScale { get; set; }
        int ModeIndex { get; set; }

        float NormalOpacity { get; set; }
        float ClickingOpacity { get; set; }

        long TotalClicks { get; set; }
        bool Welcomed { get; set; }
        bool OpenedExtraOptions { get; set; }

        T Get<T>(string propertyName, T fallback);
        void Set<T>(string propertyName, T value);
        void Save();
        void UpgradeFromPreviousVersionIfFirstRun();
    }
}
