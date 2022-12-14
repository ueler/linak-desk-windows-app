namespace LinakDeskController.LinakDesk;

public enum ToastAction
{
    MoveToStandingHeight,
    MoveToSittingHeight,
    Skip
}

static class ToastActionMethods
{
    public static string GetActionText(ToastAction action)
    {
        switch (action)
        {
            case ToastAction.MoveToStandingHeight:
                return "Time to stand";
            case ToastAction.MoveToSittingHeight:
                return "Time to sit";
            default:
                return "";
        }
    }
}