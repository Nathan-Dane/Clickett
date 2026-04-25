using System;
using Clickett.Services.Interfaces;
using s = Clickett.Properties.Settings;

namespace Clickett.Services
{
    public sealed class SettingsService : ISettingsService
    {
        public T Get<T>(string propertyName, T fallback)
        {
            try
            {
                object? value = s.Default[propertyName];
                return value is T typedValue ? typedValue : fallback;
            }
            catch
            {
                return fallback;
            }
        }

        public void Set<T>(string propertyName, T value)
        {
            try
            {
                s.Default[propertyName] = value;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not set setting '{propertyName}'.", ex);
            }
        }

        public void Save()
        {
            s.Default.Save();
        }
    }
}
