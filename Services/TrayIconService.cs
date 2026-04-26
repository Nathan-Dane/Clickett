using System;
using System.Reflection;
using Forms = System.Windows.Forms;
using Clickett.Services.Interfaces;

namespace Clickett.Services
{
    public sealed class TrayIconService : ITrayIconService
    {
        private Forms.NotifyIcon? _trayIcon;

        public bool IsVisible => _trayIcon is not null;

        public event EventHandler? ActivateRequested;
        public event EventHandler? OpenRequested;
        public event EventHandler? ExitRequested;

        public void Show(bool isActive)
        {
            if (_trayIcon is not null)
            {
                SetActiveState(isActive);
                return;
            }

            _trayIcon = new Forms.NotifyIcon
            {
                Text = "Clickett",
                ContextMenuStrip = new Forms.ContextMenuStrip(),
                Visible = true
            };

            _trayIcon.ContextMenuStrip.Font = new System.Drawing.Font("LEMON MILK Pro FTR", 10);
            _trayIcon.ContextMenuStrip.ImageScalingSize = System.Drawing.Size.Empty;
            _trayIcon.ContextMenuStrip.BackColor = System.Drawing.Color.FromArgb(255, 69, 170, 150);
            _trayIcon.ContextMenuStrip.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);

            _trayIcon.ContextMenuStrip.Items.Add(
                isActive ? "Deactivate" : "Activate",
                null,
                (_, _) => ActivateRequested?.Invoke(this, EventArgs.Empty));

            _trayIcon.ContextMenuStrip.Items.Add(
                "Open",
                null,
                (_, _) => OpenRequested?.Invoke(this, EventArgs.Empty));

            _trayIcon.ContextMenuStrip.Items.Add(
                "Exit",
                null,
                (_, _) => ExitRequested?.Invoke(this, EventArgs.Empty));

            _trayIcon.DoubleClick += (_, _) =>
                ActivateRequested?.Invoke(this, EventArgs.Empty);

            SetActiveState(isActive);
        }

        public void Hide()
        {
            _trayIcon?.Dispose();
            _trayIcon = null;
        }

        public void SetActiveState(bool isActive)
        {
            if (_trayIcon is null) return;

            string basePath = System.IO.Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location) ?? string.Empty;

            _trayIcon.Icon = new System.Drawing.Icon(
                System.IO.Path.Combine(
                    basePath,
                    isActive ? @"res\iconcirc.ico" : @"res\iconcircbw.ico"));

            if (_trayIcon.ContextMenuStrip?.Items.Count > 0)
                _trayIcon.ContextMenuStrip.Items[0].Text = isActive ? "Deactivate" : "Activate";
        }

        public void Dispose()
        {
            Hide();
        }
    }
}
