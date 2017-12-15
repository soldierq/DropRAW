

using System;

namespace DropRAW
{
    class ConsoleOutput
    {
        public void Error(Exception ex)
        {
            Error(ex.Message);
        }

        public void Error(string ex)
        {
            Console.WriteLine(string.Format("[ERROR] {0}", ex));
        }

        public void Info(string info)
        {
            Console.WriteLine(string.Format("{0}", info));
        }
    }
}
