using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBLE
{
    class BleCommand
    {
        DeviceManager devMan;
        AdvertisementManager AdvMan;

        public int start(string[] args)
        {
            if (devMan == null)
            {
                devMan = new DeviceManager();
                devMan.StartScan();
                BleConsole.LogWrite("Server started Success.");
            }
            else
                BleConsole.LogWrite("Server is running, stop first.");
            return 0;
        }

        public  int stop(string[] args)
        {
            if (devMan == null)
            {
                BleConsole.LogWrite("Error: service is stoped, start first.");
                return -1;
            }
            devMan.StopScan();
            devMan = null;
            return 0;
        }

        public int list(string[] args)
        {
            if (devMan == null)
            {
                BleConsole.LogWrite("Error: service is stoped, start first.");
                return -1;
            }
            BleConsole.LogWrite(devMan.GetList());
            return 0;
        }

        public int connect(string[] args)
        {
            if (args.Count() < 2)
            {
                BleConsole.LogWrite("Error: paramater");
                return -1;
            }

            if (devMan == null)
            {
                BleConsole.LogWrite("Error: service is stoped, start first.");
                return -1;
            }

            if(devMan.Connect(args[1]))
                BleConsole.LogWrite("Success to connect devices:" + args[1]);
            else
                BleConsole.LogWrite("Failed to connect devices:" + args[1]);

            return 0;
        }

        public int disconnect(string[] args)
        {
            if (devMan == null && args.Count() < 2)
            {
                BleConsole.LogWrite("Error: service is stoped, start first.");
                return -1;
            }

            devMan.DisConnect();
            return 0;
        }

        public int write(string[] args)
        {
            if(args.Count() < 2)
            {
                BleConsole.LogWrite("Error: paramater");
                return -1;
            }

            if (devMan == null )
            {
                BleConsole.LogWrite("Error: service is stoped");
                return -1;
            }

            devMan.Write(args[1]);
            return 0;
        }

        public int notify(string[] args)
        {
            if (devMan == null)
            {
                BleConsole.LogWrite("Error: service is stoped");
                return -1;
            }
            BleConsole.LogWrite("Notify Device.");
            devMan.Notify();
            return 0;
        }

        public int listen(string[] args)
        {
            AdvMan = new AdvertisementManager();
            BleConsole.LogWrite("Listener started Success!");
            return 0;
        }

        public int filter(string[] args)
        {
            foreach(var s in args)
            {
                BleConsole.LogWrite("param:" + s);
            }

            if (args.Count() < 2)
            {
                BleConsole.LogWrite("Error: paramater");
                return -1;
            }

            if (AdvMan == null)
            {
                BleConsole.LogWrite("Error: Listener is stoped");
                return -1;
            }
            BleConsole.LogWrite("Set Advertisement filter:" + args[1]);
            AdvMan.FilterByName(args[1]);
            return 0;
        }

    }
}
