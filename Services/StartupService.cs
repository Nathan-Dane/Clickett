using Microsoft.Win32;
using System.Reflection;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class StartupService : IStartupService
    {
        private const string RunKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string AppName = "Clickett";

        public void EnableStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);

            key?.SetValue(
                AppName,
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Clickett.exe",
                RegistryValueKind.ExpandString);
        }

        public void DisableStartup()
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);
            key?.DeleteValue(AppName, false);
        }
    }
}
