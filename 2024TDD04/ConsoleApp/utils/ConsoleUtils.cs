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

        public static Boolean IsIntValidation(string input)
        {
            return int.TryParse(input, out _);
        }

        public static string WaitInput(Func<string, Boolean> validation, string errorMessage)
        {
            string output = "";
            Boolean isValid = false;
            while (!isValid)
            {
                // print '>' character
                Console.Write("> ");
                // wait on user input
                output = Console.ReadLine();
                // validate
                isValid = validation(output);
                // if is not valid
                if (!isValid)
                    Console.WriteLine(errorMessage);
            }
            return output;
        }
        

        public static void Title(string title)
        {
            Console.WriteLine($"Begining the \"{title}\" process...");
        }

        public static void Success(string message)
        {
            Console.WriteLine(Colors.Colorize(message, Colors.Green));
        }

        public static void Error(string message)
        {
            Console.WriteLine(Colors.Colorize(message, Colors.Red));
        }

        public static void Info(string message)
        {
            Console.WriteLine(Colors.Colorize(message, Colors.Blue));
        }

        public static void Warning(string message)
        {
            Console.WriteLine(Colors.Colorize(message, Colors.Yellow));
        }
    }
}
