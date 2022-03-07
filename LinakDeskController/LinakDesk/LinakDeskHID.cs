using Nito.AsyncEx;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LinakDeskController
{
    internal class LinakDeskHID
    {
        // HID device spec
        private const ushort VENDOR_ID = 0x12D3;
        private const ushort PRODUCT_ID = 0x0002;
        private const ushort USAGE_PAGE = 0xFF00; // vendor specific
        private const ushort USAGE_ID = 0x0001; // usage 1

        // HID report ids
        private const ushort GET_STATUS = 0x0304;
        private const ushort SET_HEIGHT = 0x0305;


        private static readonly AsyncLazy<HidDevice> linakDeskHid = new AsyncLazy<HidDevice>(
            async () =>
            {
                string selector = HidDevice.GetDeviceSelector(USAGE_PAGE, USAGE_ID, VENDOR_ID, PRODUCT_ID);
                var devices = await DeviceInformation.FindAllAsync(selector);
                return await HidDevice.FromIdAsync(devices.ElementAt(0).Id, FileAccessMode.Read);
            }
        );

        public static AsyncLazy<HidDevice> getLinkDeskHid()
        {
            return linakDeskHid;
        }

        public async static Task<short> getDeskHeight()
        {
            HidDevice device = await LinakDeskHID.getLinkDeskHid();

            HidFeatureReport report = await device.GetFeatureReportAsync(GET_STATUS);
            DataReader dataReader = DataReader.FromBuffer(report.Data);
            byte[] bytes = new byte[report.Data.Length];
            dataReader.ReadBytes(bytes);

            return BitConverter.ToInt16(bytes, 4);
        }

        public async static Task<short> setDeskHeight(short height)
        {
            HidDevice device = await LinakDeskHID.getLinkDeskHid();
            HidFeatureReport report = device.CreateFeatureReport(SET_HEIGHT);

            byte[] bytes = new byte[64];
            byte heightByte1 = Convert.ToByte(height & 0x00FF);
            byte heightByte2 = Convert.ToByte((height & 0xFF00) >> 8);

            bytes.SetValue(Convert.ToByte(0x05), 0);
            for(int i = 0; i < 4; i++)
            {
                bytes.SetValue(heightByte1, i*2 + 1);
                bytes.SetValue(heightByte2, i*2 + 2);
            }

            report.Data = bytes.AsBuffer();

            return (short) (await device.SendFeatureReportAsync(report));
        }

    }
}
