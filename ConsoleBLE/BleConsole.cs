using System;
using System.Threading;
using System.Reflection;

// This example code shows how you could implement the required main function for a 
// Console UWP Application. You can replace all the code inside Main with your own custom code.

// You should also change the Alias value in the AppExecutionAlias Extension in the 
// Package.appxmanifest to a value that you define. To edit this file manually, right-click
// it in Solution Explorer and select View Code, or open it with the XML Editor.

namespace ConsoleBLE
{
    class BleConsole
    {
        private BleCommand com = new BleCommand();
        static private string InputBuf = "";

        static public void LogWrite(string log)
        {
            //Console.CursorTop = System.Console.CursorTop - 1;
            Console.Write("\r" + new string(' ', Console.BufferWidth));
            Console.CursorTop = Console.CursorTop - 1;
            Console.Write("\r" + log + '\n');
            Console.Write("Console\\>" + InputBuf);
        }

        public void RunCommand(string[] args)
        {
            string cmd = args[0].ToLower();

            MethodInfo method = typeof(BleCommand).GetMethod(cmd);

            if( method != null)
            {
                method.Invoke(com, new object[1]{ args});
            }
            else
            {
                LogWrite("Error Command!");
            }
        }

        public void Loop(string[] args)
        {
            Console.Write("console\\>");
            while (true)
            {
                InputBuf += Console.ReadKey().KeyChar;
                if(InputBuf.EndsWith('\r'))
                {
                    string cmd = InputBuf;
                    InputBuf = "";
                    Console.Write("\nconsole\\>");
                    RunCommand(cmd.Split());
                }
            }
        }
    }
}
