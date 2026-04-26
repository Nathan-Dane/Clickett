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

        public ICommand OpenHelpCommand { get; }
        public ICommand OpenGithubCommand { get; }
        public ICommand OpenContactCommand { get; }
        public ICommand OpenWarningCommand { get; }
        public ICommand OpenSupportCommand { get; }
        public ICommand ShowNotificationCommand { get; }
    }
}
