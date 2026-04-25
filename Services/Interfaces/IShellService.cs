namespace Clickett.Services.Interfaces
{
    public interface IShellService
    {
        void OpenUrl(string url);
        void OpenEmail(string mailtoUrl);
    }
}
