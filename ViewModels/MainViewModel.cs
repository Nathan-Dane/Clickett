using System.Reflection;
using System.Windows.Input;
using Clickett.Commands;
using Clickett.Services.Interfaces;

namespace Clickett.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;
        private readonly INotificationService _notificationService;
        private readonly IShellService _shellService;

        private bool _isActive;
        private bool _isClicking;
        private bool _isSettingsOpen;
        private string _hudText = string.Empty;

        //Click Profile
        private bool _jitter;
        private bool _doubleClick;
        private bool _countTotal;
        private bool _alwaysOnTop;


        public MainViewModel(
            ISettingsService settingsService,
            INotificationService notificationService,
            IShellService shellService)
        {
            _settingsService = settingsService;
            _notificationService = notificationService;
            _shellService = shellService;

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

            ShowNotificationCommand = new RelayCommand(_ =>
                _notificationService.Show("Clickett", "Notification service is working."));

            // Click Profile
            ToggleJitterCommand = new RelayCommand(() => Jitter = !Jitter);
            ToggleDoubleClickCommand = new RelayCommand(() => DoubleClick = !DoubleClick);
            ToggleCountTotalCommand = new RelayCommand(() => CountTotal = !CountTotal);
            ToggleAlwaysOnTopCommand = new RelayCommand(() => AlwaysOnTop = !AlwaysOnTop);
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

    }
}
