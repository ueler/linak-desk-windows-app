using Windows.Storage;

namespace LinakDeskController
{
    public class LinakDeskControllerSettings
    {

        public short standingHeight { get; set; } = 70;
        public short sittingHeight { get; set; } = 20;

        public short mainIntervalInMinutes { get; set; } = 60;

        public short standingIntervalInMinutes { get; set; } = 15;


        public void saveSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["standingHeight"] = this.standingHeight;
            localSettings.Values["sittingHeight"] = this.sittingHeight;
            localSettings.Values["mainIntervalInMinutes"] = this.mainIntervalInMinutes;
            localSettings.Values["standingIntervalInMinutes"] = this.standingIntervalInMinutes;
        }

        public void loadSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values["standingHeight"] != null)
            {
                this.standingHeight = (short)localSettings.Values["standingHeight"];
            }

            if (localSettings.Values["sittingHeight"] != null)
            {
                this.sittingHeight = (short)localSettings.Values["sittingHeight"];
            }

            if (localSettings.Values["mainIntervalInMinutes"] != null)
            {
                this.mainIntervalInMinutes = (short)localSettings.Values["mainIntervalInMinutes"];
            }

            if (localSettings.Values["standingIntervalInMinutes"] != null)
            {
                this.standingIntervalInMinutes = (short)localSettings.Values["standingIntervalInMinutes"];
            }
        }

    }
}
