using System;
using System.Threading.Tasks;

namespace Clickett.Services.Interfaces
{
    public interface IClickService
    {
        bool IsClicking { get; }

        event EventHandler<long>? ClickCountChanged;

        Task StartAsync();
        void Stop();
    }
}
