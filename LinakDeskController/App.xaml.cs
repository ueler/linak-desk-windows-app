using System;
using System.Reactive.Linq;
using LinakDeskController.LinakDesk;
using LinakDeskController.Toast;
using Microsoft.UI.Xaml;

namespace LinakDeskController
{
    public partial class App
    {
        private readonly LinakDeskCommandCoordinator _linakDeskCommandCoordinator = new();
        private readonly LinakDeskControllerSettings _settings = new();
        private readonly DeskStatusTracker _deskStatusTracker = new();
        private MainWindow _mWindow;
        private ToastManager _toastManager;

        public App()
        {
            this.InitializeComponent();
            GlobalHotKey.RegisterHotKey("Ctrl + Win + Up",
                () => _linakDeskCommandCoordinator.MoveToStandingHeight(_settings));
            GlobalHotKey.RegisterHotKey("Ctrl + Win + Down",
                () => _linakDeskCommandCoordinator.MoveToSittingHeight(_settings));
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _settings.LoadSettings();
            _linakDeskCommandCoordinator.Run();

            _mWindow = new MainWindow(_linakDeskCommandCoordinator, _settings);
            _mWindow.Activate();

            _toastManager = new ToastManager(_mWindow, _linakDeskCommandCoordinator, _settings);
            _toastManager.Init();

            Observable
                .Interval(TimeSpan.FromMinutes(1))
                .Subscribe(_ =>
                {
                    _deskStatusTracker.ReportStatus(_linakDeskCommandCoordinator.GetDeskStatus(_settings));

                    if (_deskStatusTracker.OverSettingsSittingTime(_settings))
                    {
                        _toastManager.ShowToastForNewDeskStatus(ToastAction.MoveToStandingHeight);
                        _deskStatusTracker.ResetTime();
                    }

                    if (_deskStatusTracker.OverSettingsStandingTime(_settings))
                    {
                        _toastManager.ShowToastForNewDeskStatus(ToastAction.MoveToSittingHeight);
                        _deskStatusTracker.ResetTime();
                    }
                });
        }
    }
}