using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;


namespace ConsoleBLE
{


    class BleDevices
    {
        private Guid NORTIC_UART_SERVER_UUID = new Guid("6e400001-b5a3-f393-e0a9-e50e24dcca9e");
        private DeviceInformation info;
        private BluetoothLEDevice bluetoothLeDevice;
        private List<GattCharacteristic> Characteristics = new List<GattCharacteristic>();

        public int Rssi;

        public string Id
        {
            get
            {
                return info.Id;
            }
        }

        public string Name
        {
            get
            {
                return info.Name;
            }
        }

        public BleDevices(DeviceInformation deviceInfo)
        {
            info = deviceInfo;
            Rssi = (int)deviceInfo.Properties["System.Devices.Aep.SignalStrength"];
        }

        public async void Connect()
        {
            if (bluetoothLeDevice != null)
                return;

            // Note: BluetoothLEDevice.FromIdAsync must be called from a UI thread because it may prompt for consent.
            bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(Id);

            // Emun service & characteristic
            var ServicesResult = await bluetoothLeDevice.GetGattServicesAsync();

            if (ServicesResult.Status == GattCommunicationStatus.Success)
            {
                var services = ServicesResult.Services;

                foreach (var service in services)
                {

                    //Console.WriteLine("service:" + service.Uuid);
                    //is a uart guid?
                    if (service.Uuid != NORTIC_UART_SERVER_UUID)
                        continue;

                    var CharacteristicsResult = await service.GetCharacteristicsAsync();

                    if (CharacteristicsResult.Status == GattCommunicationStatus.Success)
                    {
                        var characteristics = CharacteristicsResult.Characteristics;

                        foreach (var characteristic in characteristics)
                        {
                            var properties = characteristic.CharacteristicProperties;

                            Console.WriteLine("----characteristic:" + characteristic.Uuid);

                            Characteristics.Add(characteristic);
                        }
                    }
                }
            }
            return;
        }

        private GattCharacteristic FindCharacteristicByFlag(Enum flag)
        {
            foreach (var characteristic in Characteristics)
            {
                if (characteristic.CharacteristicProperties.HasFlag(flag))
                    return characteristic;
            }
            return null;
        }


        private void Characteristic_ValueChanged(GattCharacteristic sender,
                                    GattValueChangedEventArgs args)
        {
            // An Indicate or Notify reported that the value has changed.
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            // Parse the data however required.
            //Console.WriteLine(reader.ReadInt16().ToString() + ":" + reader.ReadInt16().ToString() + ":" + reader.ReadInt16().ToString());
            Console.Write(reader.ReadString(reader.UnconsumedBufferLength));
        }

        public async void Notify()
        {
            GattCharacteristic characteristic = FindCharacteristicByFlag(GattCharacteristicProperties.Notify);
            if(characteristic != null)
            {
                GattCommunicationStatus status = await characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                       GattClientCharacteristicConfigurationDescriptorValue.Notify);

                if (status == GattCommunicationStatus.Success)
                {
                    // Server has been informed of clients interest.
                    characteristic.ValueChanged += Characteristic_ValueChanged;
                }
            }

        }

        public async void Write(string value)
        {
            var writer = new DataWriter();

            GattCharacteristic characteristic = FindCharacteristicByFlag(GattCharacteristicProperties.Write);

            if (characteristic != null)
            {
                // WriteByte used for simplicity. Other commmon functions - WriteInt16 and WriteSingle
                writer.WriteString(value);

                var result = await characteristic.WriteValueAsync(writer.DetachBuffer());
                if (result == GattCommunicationStatus.Success)
                {
                    return;
                }
            }
        }

        public void Disconnect()
        {
            if (bluetoothLeDevice != null)
            {
                bluetoothLeDevice.Dispose();
                bluetoothLeDevice = null;
            }
            return;
        }
    }
}
