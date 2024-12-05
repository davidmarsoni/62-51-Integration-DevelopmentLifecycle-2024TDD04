using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.utils
{
    public class ConsoleUtils
    {
        public static Boolean ExitOnInputExit(string input, string exitText) {
            // if the string is 'exit'
            if (input.ToLower() == "exit")
            {
                if (!String.IsNullOrEmpty(exitText))
                    Console.WriteLine(Colors.Colorize(exitText, Colors.Blue));
                return true;
            }
            return false;
        }

        public static Boolean EmptyValidationMethod(string input) {
            return true;
        }
    }
}
