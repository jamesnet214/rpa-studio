using Avalon.Windows.Controls;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RoslynPad;
using RoslynPad.Editor;
using RoslynPad.Roslyn;
using RoslynPad.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Composition.Hosting;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WPFUI.Appearance;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace ProjectStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModelBase _viewModel;
        private bool _isClosing;
        private bool _isClosed;

     
        public MainWindow()
        {
            //Loaded += OnLoaded;

            //var services = new ServiceCollection();
            //services.AddLogging(l => l.AddSimpleConsole().AddDebug());

            //var container = new ContainerConfiguration()
            //    .WithProvider(new ServiceCollectionExportDescriptorProvider(services))
            //    .WithAssembly(typeof(MainViewModelBase).Assembly)   // RoslynPad.Common.UI
            //    .WithAssembly(typeof(MainWindow).Assembly);         // RoslynPad
            //var locator = container.CreateContainer().GetExport<IServiceProvider>();

            //_viewModel = locator.GetRequiredService<MainViewModelBase>();

            //DataContext = _viewModel;

            InitializeComponent();
            InitializeUi();





        }


        #region Roslyn

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            //await _viewModel.Initialize().ConfigureAwait(false);
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!_isClosing)
            {
                SaveDockLayout();
                SaveWindowLayout();

                _isClosing = true;
                IsEnabled = false;
                e.Cancel = true;

                try
                {
                    await Task.Run(() => _viewModel.OnExit()).ConfigureAwait(true);
                }
                catch
                {
                    // ignored
                }

                _isClosed = true;
                var closeTask = Dispatcher.InvokeAsync(Close);
            }
            else
            {
                e.Cancel = !_isClosed;
            }
        }

        private void LoadWindowLayout()
        {
            var boundsString = _viewModel.Settings.WindowBounds;
            if (!string.IsNullOrEmpty(boundsString))
            {
                try
                {
                    var bounds = Rect.Parse(boundsString);
                    if (bounds != default)
                    {
                        Left = bounds.Left;
                        Top = bounds.Top;
                        Width = bounds.Width;
                        Height = bounds.Height;
                    }
                }
                catch (FormatException)
                {
                }
            }

            if (Enum.TryParse(_viewModel.Settings.WindowState, out WindowState state) &&
                state != WindowState.Minimized)
            {
                WindowState = state;
            }

            if (_viewModel.Settings.WindowFontSize.HasValue)
            {
                FontSize = _viewModel.Settings.WindowFontSize.Value;
            }
        }

        private void SaveWindowLayout()
        {
            _viewModel.Settings.WindowBounds = RestoreBounds.ToString(CultureInfo.InvariantCulture);
            _viewModel.Settings.WindowState = WindowState.ToString();
        }

        private void LoadDockLayout()
        {
            //var layout = _viewModel.Settings.DockLayout;
            //if (string.IsNullOrEmpty(layout)) return;

            //var serializer = new XmlLayoutSerializer(DockingManager);
            //var reader = new StringReader(layout);
            //try
            //{
            //    serializer.Deserialize(reader);
            //}
            //catch
            //{
            //    // ignored
            //}
        }

        private void SaveDockLayout()
        {
            //var serializer = new XmlLayoutSerializer(DockingManager);
            //var document = new XDocument();
            //using (var writer = document.CreateWriter())
            //{
            //    serializer.Serialize(writer);
            //}
            //document.Root?.Element("FloatingWindows")?.Remove();
            //_viewModel.Settings.DockLayout = document.ToString();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private async void DockingManager_OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            e.Cancel = true;
            var document = (OpenDocumentViewModel)e.Document.Content;
            await _viewModel.CloseDocument(document).ConfigureAwait(false);
        }

        private void ViewErrorDetails_OnClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.LastError == null) return;

            TaskDialog.ShowInline(this, "Unhandled Exception",
                _viewModel.LastError.ToString(), string.Empty, TaskDialogButtons.Close);
        }

        private void ViewUpdateClick(object sender, RoutedEventArgs e)
        {
            _ = Task.Run(() => Process.Start(new ProcessStartInfo("https://roslynpad.net/") { UseShellExecute = true }));
        }

        #endregion
















        #region WPFUI
        private void InitializeUi()
        {
            Loaded += (sender, args) =>
            {
                // After loading the main application window,
                // we register the Watcher class, which automatically
                // changes the theme and accent of the application.
                Watcher.Watch(this, WPFUI.Appearance.BackgroundType.Mica, true, true);

#if DEBUG
                // If we are in debug mode,
                // we add an additional page in the navigation.
                // RootNavigation.Items.Add(new WPFUI.Controls.NavigationItem
                // {
                //     Page = typeof(Pages.Debug),
                //     Content = "Debug",
                //     Icon = WPFUI.Common.SymbolRegular.Warning24,
                //     IconForeground = System.Windows.Media.Brushes.Red,
                //     IconFilled = true
                // });
#endif
            };
        }


        private void InvokeSplashScreen()
        {
            RootMainGrid.Visibility = Visibility.Collapsed;
            RootWelcomeGrid.Visibility = Visibility.Visible;

            Task.Run(async () =>
            {
                // Remember to always include Delays and Sleeps in
                // your applications to be able to charge the client for optimizations later.
                await Task.Delay(4000);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    RootWelcomeGrid.Visibility = Visibility.Hidden;
                    RootMainGrid.Visibility = Visibility.Visible;
                });
            });
        }

        private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
        {
            // We check what theme is currently
            // active and choose its opposite.
            var newTheme = WPFUI.Appearance.Theme.GetAppTheme() == WPFUI.Appearance.ThemeType.Dark
                ? WPFUI.Appearance.ThemeType.Light
                : WPFUI.Appearance.ThemeType.Dark;

            // We apply the theme to the entire application.
            WPFUI.Appearance.Theme.Apply(
                themeType: newTheme,
                backgroundEffect: WPFUI.Appearance.BackgroundType.Mica,
                updateAccent: true,
                forceBackground: false);
        }

        private void TrayMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem menuItem)
                return;

            System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Tray clicked: {menuItem.Tag}", "WPFUI.Demo");
        }

        private void RootNavigation_OnNavigated(INavigation sender, RoutedNavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"DEBUG | WPF UI Navigated to: {e.CurrentPage.PageTag}", "WPFUI.Demo");

            // This funky solution allows us to impose a negative
            // margin for Frame only for the Dashboard page, thanks
            // to which the banner will cover the entire page nicely.
            RootFrame.Margin = new Thickness(
                left: 0,
                top: e.CurrentPage.PageTag == "dashboard" ? -69 : 0,
                right: 0,
                bottom: 0);
        }

        private void RootDialog_OnButtonRightClick(object sender, RoutedEventArgs e)
        {
            RootDialog.Hide();
        }



        #endregion











    }
}
