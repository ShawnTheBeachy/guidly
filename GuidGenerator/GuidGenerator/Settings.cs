using System;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Guidly
{
    public class Settings
    {
        public static Settings Instance { get; } = new Settings();

        public ApplicationTheme AppTheme
        {
            get
            {
                var theme = ApplicationTheme.Light;
                var value = Read(nameof(AppTheme), theme.ToString());
                return Enum.TryParse(value, out theme) ? theme : ApplicationTheme.Light;
            }
            set
            {
                Write(nameof(AppTheme), value.ToString());
                (Window.Current.Content as FrameworkElement).RequestedTheme = value == ApplicationTheme.Dark ? ElementTheme.Dark : ElementTheme.Light;
            }
        }

        private T Read<T>(string key, T defaultValue)
        {
            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                return (T)ApplicationData.Current.LocalSettings.Values[key];
            else
                return defaultValue;
        }

        private void Write(string key, object value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }
    }
}
