using System;
using LinakDeskController.LinakDesk;
using Microsoft.Toolkit.Uwp.Notifications;

namespace LinakDeskController.Toast;

public class ToastManager
{
    private readonly MainWindow _mainWindow;
    private readonly LinakDeskCommandCoordinator _linakDeskCommandCoordinator;
    private readonly LinakDeskControllerSettings _settings;

    public ToastManager(MainWindow mainWindow,
        LinakDeskCommandCoordinator linakDeskCommandCoordinator,
        LinakDeskControllerSettings settings)
    {
        _mainWindow = mainWindow;
        _linakDeskCommandCoordinator = linakDeskCommandCoordinator;
        _settings = settings;
    }


    public void Init()
    {
        ToastNotificationManagerCompat.OnActivated += ToastNotificationManagerCompat_OnActivated;
    }


    public void ShowToastForNewDeskStatus(ToastAction toastAction)
    {
        new ToastContentBuilder()
            .AddText(ToastActionMethods.GetActionText(toastAction))
            .AddButton(new ToastButton()
                .SetContent("Skip")
                .AddArgument("action", ToastAction.Skip.ToString()))
            .AddButton(new ToastButton()
                .SetContent("Move")
                .AddArgument("action", toastAction.ToString()))
            .SetToastDuration(ToastDuration.Long)
            .Show(toast => { toast.ExpirationTime = DateTime.Now.AddMinutes(2); });
    }

    private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
    {
        var dispatcherQueue = _mainWindow?.DispatcherQueue;

        dispatcherQueue.TryEnqueue(delegate
        {
            var args = ToastArguments.Parse(e.Argument);
            var actionString = args["action"];
            var toastAction = (ToastAction)Enum.Parse(typeof(ToastAction), actionString);

            switch (toastAction)
            {
                case ToastAction.MoveToSittingHeight:
                    _linakDeskCommandCoordinator.MoveToSittingHeight(_settings);
                    break;
                case ToastAction.MoveToStandingHeight:
                    _linakDeskCommandCoordinator.MoveToStandingHeight(_settings);
                    break;
                case ToastAction.Skip:
                    break;
            }
        });
    }
}