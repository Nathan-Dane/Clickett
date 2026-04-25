namespace Clickett.Services.Interfaces
{
    public interface ISettingsService
    {
        T Get<T>(string propertyName, T fallback);
        void Set<T>(string propertyName, T value);
        void Save();
    }
}
