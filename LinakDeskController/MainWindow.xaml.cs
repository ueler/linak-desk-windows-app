using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LinakDeskController
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private LinakDeskCommandCoordinator linakDeskCommandCoordinator = new LinakDeskCommandCoordinator();

        private LinakDeskControllerSettings settings;

        private bool settingsApplied = false;

        Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();

        public MainWindow(LinakDeskControllerSettings settings)
        {
            this.settings = settings;

            this.InitializeComponent();
            this.resizeWindow();
            this.applySettings();

            this.linakDeskCommandCoordinator.getHeightSubject().Subscribe(height =>
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    currentHeight.Text = "Current table height: " + Math.Round(height / 100.0) + " cm";
                });
            });
            this.linakDeskCommandCoordinator.run();
        }

        private void applySettings()
        {
            this.sittingHeightNumberBox.Value = this.settings.sittingHeight;
            this.standingHeightNumberBox.Value = this.settings.standingHeight;
            this.mainIntervalNumberBox.Value = this.settings.mainIntervalInMinutes;
            this.standingIntervalNumberBox.Value = this.settings.standingIntervalInMinutes;
            this.settingsApplied = true;
        }

        private void resizeWindow()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = 600;
            size.Height = 600;

            appWindow.Resize(size);
        }

        private void moveToStandingHeight_Click(object sender, RoutedEventArgs e)
        {
            this.linakDeskCommandCoordinator.setDeskTargetHeight(this.settings.standingHeight);
        }

        private void moveToSittingHeight_Click(object sender, RoutedEventArgs e)
        {
            this.linakDeskCommandCoordinator.setDeskTargetHeight(this.settings.sittingHeight);
        }

        private void standingHeightNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!settingsApplied)
            {
                return;
            }
            this.settings.standingHeight = Convert.ToInt16(args.NewValue);
            this.settings.saveSettings();
        }

        private void sittingHeightNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!settingsApplied)
            {
                return;
            }
            this.settings.sittingHeight = Convert.ToInt16(args.NewValue);
            this.settings.saveSettings();
        }

        private void mainIntervalNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!settingsApplied)
            {
                return;
            }
            this.settings.mainIntervalInMinutes = Convert.ToInt16(args.NewValue);
            this.settings.saveSettings();
        }

        private void standingIntervalNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!settingsApplied)
            {
                return;
            }
            this.settings.standingIntervalInMinutes = Convert.ToInt16(args.NewValue);
            this.settings.saveSettings();
        }
    }
}
