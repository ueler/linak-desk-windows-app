using System;

namespace LinakDeskController.LinakDesk;

public class DeskStatusTracker
{
    private DateTime _statusStart = DateTime.Now;

    private DeskStatus _deskStatus = DeskStatus.OnSittingHeight;

    public void ReportStatus(DeskStatus deskStatus)
    {
        if (_deskStatus == deskStatus) return;
        _statusStart = DateTime.Now;
        _deskStatus = deskStatus;
    }

    public void ResetTime()
    {
        _statusStart = DateTime.Now;
    }

    public bool OverSettingsSittingTime(LinakDeskControllerSettings settings)
    {
        if (_deskStatus == DeskStatus.OnSittingHeight)
        {
            return DateTime.Now.Subtract(_statusStart).TotalMinutes > settings.SittingIntervalInMinutes;
        }

        return false;
    }

    public bool OverSettingsStandingTime(LinakDeskControllerSettings settings)
    {
        if (_deskStatus == DeskStatus.OnStandingHeight)
        {
            return DateTime.Now.Subtract(_statusStart).TotalMinutes > settings.StandingIntervalInMinutes;
        }

        return false;
    }
}