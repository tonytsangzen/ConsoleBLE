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
                BleConsole.LogWrite("[Listen]" + eventArgs.Advertisement.LocalName + ":" + reader.ReadInt32().ToString());
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
