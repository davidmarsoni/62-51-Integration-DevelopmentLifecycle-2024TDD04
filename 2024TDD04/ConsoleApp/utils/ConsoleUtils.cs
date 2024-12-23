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

        public static Boolean EmptyValidation(string input) {
            return true;
        }

        public static Boolean IsIntValidation(string input)
        {
            return int.TryParse(input, out _);
        }

        public static Boolean EmptyOrIntValidation(string input)
        {
            return string.IsNullOrEmpty(input) || int.TryParse(input, out _);
        }
    }
}
