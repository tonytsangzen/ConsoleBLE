using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace ConsoleBLE
{
    class BleDevices
    {
        public string Id;
        public string Name;
        public int Rssi;

        public BleDevices(DeviceInformation deviceInfo)
        {
            Id = deviceInfo.Id;
            Name = deviceInfo.Name;
            Rssi = (int)deviceInfo.Properties["System.Devices.Aep.SignalStrength"];
        }
    }
}
