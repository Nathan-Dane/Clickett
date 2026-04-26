using Clickett.Commands;
using Clickett.Services;
using Clickett.Services.Interfaces;
using System.Reflection;
using System.Windows.Input;

namespace Clickett.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly IShellService _shellService;
        private readonly IStartupService _startupService;
        private readonly ITrayIconService _trayIconService;
        private readonly IUpdateService _updateService;


        private bool _isActive;
        private bool _isClicking;
        private bool _isSettingsOpen;
        private string _hudText = string.Empty;

        //Click Profile
        private bool _jitter;
        private bool _doubleClick;
        private bool _countTotal;
        private bool _alwaysOnTop;

        //Settings
        private bool _startup;
        private bool _trayIcon;
        private bool _minimizeToTray;

        //Update
        private bool _isUpdateAvailable;
        private int _updateProgress;


        public MainViewModel(
                ISettingsService settingsService,
                INotificationService notificationService,
                IShellService shellService,
                IStartupService startupService,
                ITrayIconService trayIconService,
                IUpdateService updateService)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
            _shellService = shellService;
            _startupService = startupService;
            _trayIconService = trayIconService;
            _updateService = updateService;

            OpenHelpCommand = new RelayCommand(() =>
                _shellService.OpenUrl("https://clickett.app/help"));

            OpenGithubCommand = new RelayCommand(() =>
                _shellService.OpenUrl("https://github.com/Nathan-Dane"));

            OpenContactCommand = new RelayCommand(() =>
                _shellService.OpenEmail("mailto:clickett.help@gmail.com?subject=Clickett%20Support"));

            OpenWarningCommand = new RelayCommand(() =>
                _shellService.OpenUrl("https://github.com/NathanDagDane/Clickett/wiki/Getting-Started,-Help-and-FAQ#only-69-clicks-per-second"));

            OpenSupportCommand = new RelayCommand(() =>
                _shellService.OpenUrl("https://nathandagdane.github.io/Clickett/Donate/"));

            ToggleStartupCommand = new RelayCommand(ToggleStartup);
            ToggleTrayIconCommand = new RelayCommand(ToggleTrayIcon);
            ToggleMinimizeToTrayCommand = new RelayCommand(ToggleMinimizeToTray);

            CheckUpdateCommand = new AsyncRelayCommand(() =>
                _updateService.CheckAndDownloadUpdateAsync(true));

            InstallUpdateCommand = new RelayCommand(() =>
                _updateService.ApplyUpdateAndRestart());

            // Update Subscribe
            _updateService.UpdateAvailable += (_, _) => IsUpdateAvailable = true;
            _updateService.DownloadProgressChanged += (_, progress) =>
            {
                UpdateProgress = progress;

                if (progress == 100)
                    IsUpdateAvailable = true;
            };
            _updateService.UpdateCheckFailed += (_, message) =>
                _notificationService.Show("Update Error", message);
            _updateService.UpdateDownloadFailed += (_, message) =>
                _notificationService.Show("Download Error", message);

        }


        public string AssemblyVersion =>
            "v" + Assembly.GetExecutingAssembly().GetName().Version + " - Beta";

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public bool IsClicking
        {
            get => _isClicking;
            set => SetProperty(ref _isClicking, value);
        }

        public bool IsSettingsOpen
        {
            get => _isSettingsOpen;
            set => SetProperty(ref _isSettingsOpen, value);
        }

        public string HudText
        {
            get => _hudText;
            set => SetProperty(ref _hudText, value);
        }

        // Click Profile

        public bool Jitter
        {
            get => _jitter;
            set => SetProperty(ref _jitter, value);
        }

        public bool DoubleClick
        {
            get => _doubleClick;
            set => SetProperty(ref _doubleClick, value);
        }

        public bool CountTotal
        {
            get => _countTotal;
            set => SetProperty(ref _countTotal, value);
        }

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => SetProperty(ref _alwaysOnTop, value);
        }

        // Settings
        public bool Startup
        {
            get => _startup;
            set => SetProperty(ref _startup, value);
        }

        public bool TrayIcon
        {
            get => _trayIcon;
            set => SetProperty(ref _trayIcon, value);
        }

        public bool MinimizeToTray
        {
            get => _minimizeToTray;
            set => SetProperty(ref _minimizeToTray, value);
        }

        private void ToggleStartup()
        {
            Startup = !Startup;

            if (Startup)
                _startupService.EnableStartup();
            else
                _startupService.DisableStartup();

            _settingsService.Set("startup", Startup);
            _settingsService.Save();
        }

        private void ToggleTrayIcon()
        {
            TrayIcon = !TrayIcon;

            if (TrayIcon)
                _trayIconService.Show(false);
            else
                _trayIconService.Hide();

            if (!TrayIcon && MinimizeToTray)
                MinimizeToTray = false;

            _settingsService.Set("trayIcon", TrayIcon);
            _settingsService.Set("minToTray", MinimizeToTray);
            _settingsService.Save();
        }

        private void ToggleMinimizeToTray()
        {
            MinimizeToTray = !MinimizeToTray;

            if (MinimizeToTray && !TrayIcon)
            {
                TrayIcon = true;
                _trayIconService.Show(false);
            }

            _settingsService.Set("trayIcon", TrayIcon);
            _settingsService.Set("minToTray", MinimizeToTray);
            _settingsService.Save();
        }

        // Update

        public bool IsUpdateAvailable
        {
            get => _isUpdateAvailable;
            set => SetProperty(ref _isUpdateAvailable, value);
        }

        public int UpdateProgress
        {
            get => _updateProgress;
            set => SetProperty(ref _updateProgress, value);
        }




        public ICommand OpenHelpCommand { get; }
        public ICommand OpenGithubCommand { get; }
        public ICommand OpenContactCommand { get; }
        public ICommand OpenWarningCommand { get; }
        public ICommand OpenSupportCommand { get; }
        public ICommand ShowNotificationCommand { get; }

        // Click Profile
        public ICommand ToggleJitterCommand { get; }
        public ICommand ToggleDoubleClickCommand { get; }
        public ICommand ToggleCountTotalCommand { get; }
        public ICommand ToggleAlwaysOnTopCommand { get; }

        // Settings
        public ICommand ToggleStartupCommand { get; }
        public ICommand ToggleTrayIconCommand { get; }
        public ICommand ToggleMinimizeToTrayCommand { get; }

        // Update
        public ICommand CheckUpdateCommand { get; }
        public ICommand InstallUpdateCommand { get; }
    }
}
