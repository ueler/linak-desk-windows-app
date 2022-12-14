using Windows.Storage;

namespace LinakDeskController.LinakDesk
{
    public class LinakDeskControllerSettings
    {
        private const string StandingHeightStr = "standingHeight";
        private const string SittingHeightStr = "sittingHeight";
        private const string SittingIntervalInMinutesStr = "sittingIntervalInMinutes";
        private const string StandingIntervalInMinutesStr = "standingIntervalInMinutes";

        public short StandingHeight { get; set; } = 60;
        public short SittingHeight { get; set; } = 20;

        public short SittingIntervalInMinutes { get; set; } = 60;

        public short StandingIntervalInMinutes { get; set; } = 15;


        public void SaveSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[StandingHeightStr] = StandingHeight;
            localSettings.Values[SittingHeightStr] = SittingHeight;
            localSettings.Values[SittingIntervalInMinutesStr] = SittingIntervalInMinutes;
            localSettings.Values[StandingIntervalInMinutesStr] = StandingIntervalInMinutes;
        }

        public void LoadSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values[StandingHeightStr] != null)
            {
                StandingHeight = (short)localSettings.Values[StandingHeightStr];
            }

            if (localSettings.Values[SittingHeightStr] != null)
            {
                SittingHeight = (short)localSettings.Values[SittingHeightStr];
            }

            if (localSettings.Values[SittingIntervalInMinutesStr] != null)
            {
                SittingIntervalInMinutes = (short)localSettings.Values[SittingIntervalInMinutesStr];
            }

            if (localSettings.Values[StandingIntervalInMinutesStr] != null)
            {
                StandingIntervalInMinutes = (short)localSettings.Values[StandingIntervalInMinutesStr];
            }
        }

    }
}
