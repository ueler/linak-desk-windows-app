using System;
using Windows.Graphics;
using LinakDeskController.LinakDesk;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;

namespace LinakDeskController
{
    public sealed partial class MainWindow : Window
    {
        private readonly LinakDeskCommandCoordinator _linakDeskCommandCoordinator;

        private readonly LinakDeskControllerSettings _settings;

        private bool _settingsApplied;

        private readonly DispatcherQueue _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        public MainWindow(LinakDeskCommandCoordinator linakDeskCommandCoordinator, LinakDeskControllerSettings settings)
        {
            _linakDeskCommandCoordinator = linakDeskCommandCoordinator;
            _settings = settings;

            InitializeComponent();
            ResizeWindow();
            ApplySettings();
            Title = "Linak Desk Controller App";

            _linakDeskCommandCoordinator.GetHeightSubject().Subscribe(height =>
            {
                _dispatcherQueue.TryEnqueue(() =>
                {
                    currentHeight.Text = "Current table height: " + Math.Round(height / 100.0) + " cm";
                });
            });
        }

        private void ApplySettings()
        {
            sittingHeightNumberBox.Value = _settings.SittingHeight;
            standingHeightNumberBox.Value = _settings.StandingHeight;
            sittingIntervalNumberBox.Value = _settings.SittingIntervalInMinutes;
            standingIntervalNumberBox.Value = _settings.StandingIntervalInMinutes;
            _settingsApplied = true;
        }

        private void ResizeWindow()
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new SizeInt32
            {
                Width = 600,
                Height = 600
            };

            appWindow.Resize(size);
        }

        private void moveToStandingHeight_Click(object sender, RoutedEventArgs e)
        {
            _linakDeskCommandCoordinator.MoveToStandingHeight(_settings);
        }

        private void moveToSittingHeight_Click(object sender, RoutedEventArgs e)
        {
            _linakDeskCommandCoordinator.MoveToSittingHeight(_settings);
        }

        private void standingHeightNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!_settingsApplied)
            {
                return;
            }
            _settings.StandingHeight = Convert.ToInt16(args.NewValue);
            _settings.SaveSettings();
        }

        private void sittingHeightNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!_settingsApplied)
            {
                return;
            }
            _settings.SittingHeight = Convert.ToInt16(args.NewValue);
            _settings.SaveSettings();
        }

        private void sittingIntervalNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!_settingsApplied)
            {
                return;
            }
            _settings.SittingIntervalInMinutes = Convert.ToInt16(args.NewValue);
            _settings.SaveSettings();
        }

        private void standingIntervalNumberBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
        {
            if (!_settingsApplied)
            {
                return;
            }
            _settings.StandingIntervalInMinutes = Convert.ToInt16(args.NewValue);
            _settings.SaveSettings();
        }
    }
}
