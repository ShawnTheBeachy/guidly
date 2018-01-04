using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Guidly
{
    public sealed partial class MainPage : Page
    {
        #region CurrentGuid

        public Guid CurrentGuid
        {
            get => (Guid)GetValue(CurrentGuidProperty);
            set => SetValue(CurrentGuidProperty, value);
        }
        
        public static readonly DependencyProperty CurrentGuidProperty =
            DependencyProperty.Register("CurrentGuid", typeof(Guid), typeof(MainPage), new PropertyMetadata(Guid.NewGuid()));

        #endregion CurrentGuid

        private bool _isCompactView = false;

        public MainPage() =>
            InitializeComponent();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CoreWindow.GetForCurrentThread().KeyDown += CoreWindow_KeyDown;
            Window.Current.Activated += Window_Activated;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            CoreWindow.GetForCurrentThread().KeyDown -= CoreWindow_KeyDown;
        }

        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.F:
                    await ToggleViewModeAsync();
                    break;
                case VirtualKey.T:
                    ToggleTheme();
                    break;
                case VirtualKey.G:
                    CurrentGuid = Guid.NewGuid();
                    CopyGuid();
                    break;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e) =>
            CopyGuid(sender);

        private void CopyGuid(object sender = null)
        {
            var dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(CurrentGuid.ToString());
            Clipboard.SetContent(dataPackage);
            CopiedHint.Visibility = Visibility.Visible;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentGuid = Guid.NewGuid();
            CopyGuid(sender);
        }

        private async void MainPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e) =>
            await ToggleViewModeAsync();

        private void ToggleTheme()
        {
            Settings.Instance.AppTheme = Settings.Instance.AppTheme == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark;
            App.CustomizeTitleBar();
        }

        private void ToggleThemeMenuItem_Click(object sender, RoutedEventArgs e) =>
            ToggleTheme();

        private async Task ToggleViewModeAsync()
        {
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(_isCompactView ? ApplicationViewMode.Default : ApplicationViewMode.CompactOverlay);
            _isCompactView = !_isCompactView;
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e) =>
            CopyGuid();
    }
}
