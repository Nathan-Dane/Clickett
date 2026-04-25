using System;
using System.Threading.Tasks;
using Clickett.Models;

namespace Clickett.Services.Interfaces
{
    public interface IClickSessionController
    {
        bool IsRunning { get; }

        event EventHandler<long>? ClickCountChanged;
        event EventHandler? SessionEnded;

        Task StartAsync(ClickProfile profile);
        void Stop();
    }
}
