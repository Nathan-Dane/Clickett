using System;
using System.Windows.Input;
using Clickett.Services.Interfaces;
using s = Clickett.Properties.Settings;

namespace Clickett.Services
{
    public sealed class SettingsService : ISettingsService
    {
        public Key Hotkey
        {
            get => Get(nameof(s.Default.hkAction), Key.Z);
            set => Set(nameof(s.Default.hkAction), value);
        }

        public bool HotkeyCtrl
        {
            get => Get(nameof(s.Default.hkCtrl), false);
            set => Set(nameof(s.Default.hkCtrl), value);
        }

        public bool HotkeyShift
        {
            get => Get(nameof(s.Default.hkShift), false);
            set => Set(nameof(s.Default.hkShift), value);
        }

        public bool HotkeyAlt
        {
            get => Get(nameof(s.Default.hkAlt), false);
            set => Set(nameof(s.Default.hkAlt), value);
        }

        public string Theme
        {
            get => Get(nameof(s.Default.Theme), "Default");
            set => Set(nameof(s.Default.Theme), value);
        }

        public bool DoAnimations
        {
            get => Get(nameof(s.Default.doAnimations), true);
            set => Set(nameof(s.Default.doAnimations), value);
        }

        public bool Jitter
        {
            get => Get(nameof(s.Default.jitter), false);
            set => Set(nameof(s.Default.jitter), value);
        }

        public bool DoubleClick
        {
            get => Get(nameof(s.Default.doubleClick), false);
            set => Set(nameof(s.Default.doubleClick), value);
        }

        public bool CountTotal
        {
            get => Get(nameof(s.Default.countTotal), true);
            set => Set(nameof(s.Default.countTotal), value);
        }

        public bool AlwaysOnTop
        {
            get => Get(nameof(s.Default.aot), false);
            set => Set(nameof(s.Default.aot), value);
        }

        public bool Startup
        {
            get => Get(nameof(s.Default.startup), false);
            set => Set(nameof(s.Default.startup), value);
        }

        public bool TrayIcon
        {
            get => Get(nameof(s.Default.trayIcon), false);
            set => Set(nameof(s.Default.trayIcon), value);
        }

        public bool MinimizeToTray
        {
            get => Get(nameof(s.Default.minToTray), false);
            set => Set(nameof(s.Default.minToTray), value);
        }

        public int ClickInterval
        {
            get => Get(nameof(s.Default.clickInterval), 1);
            set => Set(nameof(s.Default.clickInterval), value);
        }

        public int BurstCount
        {
            get => Get(nameof(s.Default.burstCount), 1);
            set => Set(nameof(s.Default.burstCount), value);
        }

        public int UiScale
        {
            get => Get(nameof(s.Default.uiScale), 100);
            set => Set(nameof(s.Default.uiScale), value);
        }

        public int ModeIndex
        {
            get => Get(nameof(s.Default.modeInt), 0);
            set => Set(nameof(s.Default.modeInt), value);
        }

        public float NormalOpacity
        {
            get => Get(nameof(s.Default.normalOpacity), 1f);
            set => Set(nameof(s.Default.normalOpacity), value);
        }

        public float ClickingOpacity
        {
            get => Get(nameof(s.Default.clickingOpacity), 1f);
            set => Set(nameof(s.Default.clickingOpacity), value);
        }

        public long TotalClicks
        {
            get => Get(nameof(s.Default.totalClicks), 0L);
            set => Set(nameof(s.Default.totalClicks), value);
        }

        public bool Welcomed
        {
            get => Get(nameof(s.Default.welcomed), false);
            set => Set(nameof(s.Default.welcomed), value);
        }

        public bool OpenedExtraOptions
        {
            get => Get(nameof(s.Default.openedExOp), false);
            set => Set(nameof(s.Default.openedExOp), value);
        }

        public T Get<T>(string propertyName, T fallback)
        {
            try
            {
                object? value = s.Default[propertyName];
                return value is T typedValue ? typedValue : fallback;
            }
            catch
            {
                return fallback;
            }
        }

        public void Set<T>(string propertyName, T value)
        {
            try
            {
                s.Default[propertyName] = value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not set setting '{propertyName}'.", ex);
            }
        }

        public void Save()
        {
            s.Default.Save();
        }

        public void UpgradeFromPreviousVersionIfFirstRun()
        {
            if (!Get(nameof(s.Default.FirstRun), false)) return;

            s.Default.Upgrade();
            Set(nameof(s.Default.FirstRun), false);
            Save();
        }
    }
}
