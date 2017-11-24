using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GuidGenerator
{
    sealed partial class App : Application
    {
        public App() =>
            InitializeComponent();
        
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ColorTitleBar();
            var rootFrame = Window.Current.Content as Frame;
            
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);

                Window.Current.Activate();
            }

            await InstallVCDFileAsync();
        }

        private async Task InstallVCDFileAsync()
        {
            if (VoiceCommandDefinitionManager.InstalledCommandDefinitions?.Count > 0)
                return;

            var vcdFile = await Package.Current.InstalledLocation.GetFileAsync("CortanaCommands.xml");
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcdFile);
        }

        private void ColorTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            var themeColor = (Color)Resources["SystemAccentColor"];
            titleBar.BackgroundColor = themeColor;
            titleBar.ButtonBackgroundColor = themeColor;
            titleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 152, 142, 133);
            titleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 113, 106, 100);
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonForegroundColor = Colors.White;
        }
    }
}
