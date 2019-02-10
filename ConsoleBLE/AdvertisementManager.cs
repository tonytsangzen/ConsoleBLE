using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Storage.Streams;

namespace ConsoleBLE
{
    class AdvertisementManager
    {
        private BluetoothLEAdvertisementWatcher watcher;

        // reverse byte order (32-bit)
        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }


        public AdvertisementManager()
        {
            watcher = new BluetoothLEAdvertisementWatcher();
            watcher.Received += OnAdvertisementReceived;
            watcher.Start();
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            foreach (var data in eventArgs.Advertisement.ManufacturerData)
            {
                DataReader reader = DataReader.FromBuffer(data.Data);
                UInt32 count = ReverseBytes(reader.ReadUInt32());
                BleConsole.LogWrite("[Listen]" + eventArgs.Advertisement.LocalName + ":" + count.ToString());
            }
        }

        public void FilterByName(string Name)
        {
            watcher.Stop();
            BluetoothLEAdvertisement filter = new BluetoothLEAdvertisement();
            filter.LocalName = Name;
            watcher.AdvertisementFilter.Advertisement = filter;
            watcher.Start();
        }
    }
}
