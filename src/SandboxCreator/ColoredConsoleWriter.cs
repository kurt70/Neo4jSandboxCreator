using System;
using System.Collections.Generic;
using System.Text;

namespace SandboxCreator
{
    public class ColoredConsoleWriter
    {
        public void WriteInfo(string msg)
        {
            Console.WriteLine(msg);

        }
        public void WriteError(string msg)
        {
            var currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ForegroundColor = currentColor;
        }
    }
}
