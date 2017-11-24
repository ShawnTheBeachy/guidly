using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GuidGenerator
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

        private void CopyButton_Click(object sender, RoutedEventArgs e) =>
            CopyGuid(sender);

        private void CopyGuid(object sender)
        {
            var dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(CurrentGuid.ToString());
            Clipboard.SetContent(dataPackage);
            CopiedFlyout.ShowAt(sender as FrameworkElement);
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentGuid = Guid.NewGuid();
            CopyGuid(sender);
        }

        private async void MainPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(_isCompactView ? ApplicationViewMode.Default : ApplicationViewMode.CompactOverlay);
            _isCompactView = !_isCompactView;
        }
    }
}
