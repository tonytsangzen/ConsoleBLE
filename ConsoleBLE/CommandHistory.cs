using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBLE
{
    class CommandHistory
    {
        private int position = 0;
        private List<string> history = new List<string>();
        
        public CommandHistory()
        {
            history.Add("start");
        }

        public void Push(string cmd)
        {
            if (cmd != history.Last())
            {
                history.Add(cmd);
                position = history.Count;
            }
        }

        public string SearchUp()
        {
            position--;
            if (position < 0)
                position = 0;

            return history[position];
        }
        
        public string SearchDown()
        {
            position++;

            if (position >= history.Count)
                position = history.Count - 1;

            return history[position];
        }

        public string List()
        {
            string ret = new string("");
            foreach(var s in history)
            {
                ret += s;
                ret += "\r\n";
            }
            return ret;
        }
    }
}
