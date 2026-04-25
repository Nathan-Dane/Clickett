using Clickett.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Clickett.Services.Interfaces
{
    public interface IClickService
    {
        bool IsClicking { get; }

        event EventHandler<long>? ClickCountChanged;
        event EventHandler? BurstCompleted;

        Task StartAsync(ClickProfile profile, CancellationToken cancellationToken);
        void Stop();
    }
}
