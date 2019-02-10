using System;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;

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
        private CommandHistory history = new CommandHistory();

        static private string InputBuf =  "";
        
        static public void LogWrite(string log)
        {
            //Console.CursorTop = System.Console.CursorTop - 1;
            Console.Write("\r" + new string(' ', Console.BufferWidth));
            Console.CursorTop = Console.CursorTop - 1;
            Console.Write("\r" + log + '\n');
            Console.Write("Console\\>" + InputBuf);
        }

        static public void Refresh()
        {
            //Console.CursorTop = System.Console.CursorTop - 1;
            Console.Write("\r" + new string(' ', Console.BufferWidth));
            Console.CursorTop = Console.CursorTop - 1;
            Console.Write("Console\\>" + InputBuf);
        }

        public bool RunCommand(string[] args)
        {
            string cmd = args[0].ToLower();

            MethodInfo method = typeof(BleCommand).GetMethod(cmd);

            if (method != null)
            {
                method.Invoke(com, new object[1] { args });
                return true;
            }
            else if (cmd == "history")
            {
                LogWrite(history.List());
                return true;
            }
            else
            {
                LogWrite("Error Command:" + cmd);
                return false;
            }
        }

        public void Loop(string[] args)
        {
            Console.Write("console\\>");
            while (true)
            {
                var KeyPress = Console.ReadKey();
                switch(KeyPress.Key)
                {
                    case ConsoleKey.Enter:
                        Console.Write("\nConsole\\>");
                        if (InputBuf.Length > 0)
                        {
                            string cmd = InputBuf;
                            InputBuf = "";
                            if (RunCommand(cmd.Split()))
                            {
                                history.Push(cmd);
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        if (InputBuf.Length > 0)
                        {
                            InputBuf = InputBuf.Remove((InputBuf.Length - 1));
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        InputBuf = history.SearchDown();
                        break;
                    case ConsoleKey.UpArrow:
                        InputBuf = history.SearchUp();
                        break;
                    default:
                        InputBuf += KeyPress.KeyChar;
                        break;
                }
                Refresh();
            }
        }
    }
}
