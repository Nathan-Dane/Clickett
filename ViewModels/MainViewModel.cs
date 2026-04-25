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

            OpenHelpCommand = new RelayCommand(OpenHelp);
            OpenGithubCommand = new RelayCommand(OpenGithub);
            OpenContactCommand = new RelayCommand(OpenContact);
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
        public ICommand ShowNotificationCommand { get; }

        private void OpenHelp()
        {
            _shellService.OpenUrl("https://clickett.app/help");
        }

        private void OpenGithub()
        {
            _shellService.OpenUrl("https://github.com/NathanDagDane");
        }

        private void OpenContact()
        {
            _shellService.OpenEmail("mailto:clickett.help@gmail.com?subject=Clickett%20Support");
        }
    }
}
