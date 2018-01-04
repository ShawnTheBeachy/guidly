using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Guidly
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            RequestedTheme = Settings.Instance.AppTheme;
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationView.PreferredLaunchViewSize = new Size(350, 275);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(350, 275));

            CustomizeTitleBar();
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
        }

        public static void CustomizeTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverBackgroundColor = Settings.Instance.AppTheme == ApplicationTheme.Dark ? Color.FromArgb(40, 255, 255, 255) : Color.FromArgb(40, 100, 100, 100);
            titleBar.ButtonPressedBackgroundColor = Settings.Instance.AppTheme == ApplicationTheme.Dark ? Color.FromArgb(80, 255, 255, 255) : Color.FromArgb(80, 100, 100, 100);
            titleBar.ForegroundColor = Settings.Instance.AppTheme == ApplicationTheme.Dark ? Colors.White : Colors.Black;
            titleBar.ButtonForegroundColor = Settings.Instance.AppTheme == ApplicationTheme.Dark ? Colors.White : Colors.Black;
            titleBar.ButtonHoverForegroundColor = Settings.Instance.AppTheme == ApplicationTheme.Dark ? Colors.White : Colors.Black;
        }
    }
}
