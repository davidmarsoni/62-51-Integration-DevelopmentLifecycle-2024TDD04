using ConsoleApp.utils;
using ConsoleApp.console;

namespace ConsoleApp.helpers
{
    public static class InputHelper
    {
        public static int PromptForInt(string fieldName, string promptMessage)
        {
            Console.WriteLine("Enter the " + Colors.Colorize(fieldName, Colors.Yellow) + ".");
            string input = ConsoleManager.WaitInput(ConsoleUtils.IsIntValidation, promptMessage);
            if (ConsoleUtils.ExitOnInputExit(input, $"Exiting {fieldName.ToLower()} process."))
                return -1;

            return int.Parse(input);
        }

        public static string PromptForString(string fieldName, string promptMessage)
        {
            Console.WriteLine("Enter a " + Colors.Colorize(fieldName, Colors.Yellow) + ".");
            string input = ConsoleManager.WaitInput(ConsoleUtils.EmptyValidation, promptMessage);
            if (ConsoleUtils.ExitOnInputExit(input, $"Exiting {fieldName.ToLower()} process."))
                return null;

            return input;
        }

        public static bool PromptForConfirmation(string promptMessage)
        {
            Console.WriteLine(Colors.Colorize(promptMessage, Colors.Red));
            Console.Write("> ");
            string confirm = Console.ReadLine().ToLower();
            return confirm == "y";
        }
    }
}
