using System;
using System.Collections.Generic;
using System.Linq;
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
using WPFUI.Appearance;
using WPFUI.Common;
using WPFUI.Controls.Interfaces;

namespace ProjectStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModelBase _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUi();

        }


        #region Roslyn



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
