using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace ConsoleBLE
{
    class DeviceManager
    {
        private DeviceWatcher deviceWatcher;
        private List<BleDevices> deviceList;
        private bool Scaning;

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {

            if ((bool)deviceInfo.Properties["System.Devices.Aep.Bluetooth.Le.IsConnectable"])
            {
                deviceList.Add(new BleDevices(deviceInfo));
            }
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {

        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            foreach (var item in deviceList)
            {
                if (item.Id == deviceInfoUpdate.Id)
                {
                    deviceList.Remove(item);
                    return;
                }
            }
        }
        
        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object e)
        {
            Scaning = false;
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object e)
        {

        }

        public void StartScan()
        {
            if (Scaning)
                return;

            // Additional properties we would like about the device.
            // Property strings are documented here https://msdn.microsoft.com/en-us/library/windows/desktop/ff521659(v=vs.85).aspx
            string[] requestedProperties = {
                "System.Devices.Aep.DeviceAddress",
                "System.Devices.Aep.IsConnected",
                "System.Devices.Aep.Bluetooth.Le.IsConnectable",
                "System.Devices.Aep.SignalStrength"
            };

            // BT_Code: Example showing paired and non-paired in a single query.
            string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

            deviceWatcher =
                    DeviceInformation.CreateWatcher(
                        aqsAllBluetoothLEDevices,
                        requestedProperties,
                        DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            deviceList = new List<BleDevices>();
            // Start the watcher. Active enumeration is limited to approximately 30 seconds.
            // This limits power usage and reduces interference with other Bluetooth activities.
            // To monitor for the presence of Bluetooth LE devices for an extended period,
            // use the BluetoothLEAdvertisementWatcher runtime class. See the BluetoothAdvertisement
            // sample for an example.
            deviceWatcher.Start();

            Scaning = true;
        }

        public void StopScan()
        {
            if (Scaning)
            {
                deviceWatcher.Stop();
                Scaning = false;
            }

        }
        
        public string GetList()
        {
            StringBuilder ret = new StringBuilder();

            foreach (BleDevices d in deviceList)
            {
                ret.Append(d.Name + ":" + d.Rssi + "\n");
            }
            return ret.ToString();
        }
    }
}
