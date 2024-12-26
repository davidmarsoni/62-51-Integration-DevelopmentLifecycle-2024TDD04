using ConsoleApp.console;

namespace ConsoleApp.utils
{
    public static class InputUtils
    {
        /// <summary>
        /// Prompts the user for an integer input and validates it.
        /// </summary>
        /// <param name="fieldName">Name of the field the user is prompted for</param>
        /// <param name="promptMessage">Message to display to the user</param>
        /// <returns>integer input of the user or -1 if the user exits the process</returns>
        public static int PromptForInt(string fieldName, string promptMessage)
        {
            Console.WriteLine("Enter the " + Colors.Colorize(fieldName, Colors.Yellow) + ".");
            string input = ConsoleUtils.WaitInput(ConsoleUtils.IsIntValidation, promptMessage);
            if (ConsoleUtils.ExitOnInputExit(input, $"Exiting {fieldName.ToLower()} process."))
                return -1;

            return int.Parse(input);
        }


        /// <summary>
        /// Prompts the user for a string input and validates it using the provided validation function.
        /// This function will keep prompting the user until the input is valid or the user exits the process.
        /// </summary>
        /// <param name="fieldName">name of the field the user is prompted for</param>
        /// <param name="promptMessage">message to display to the user</param>
        /// <param name="validation">function to validate the input with this signature: (string) => (bool, string)</param>
        /// <param name="optional">if true, the user can skip the input by pressing enter</param>
        /// <returns>tuple of bool and string. bool is true if the user has exited during the process, false otherwise. string is the message if the input is invalid, or the input of the user if it is valid.</returns>
        public static (bool, string) PromptForString(string fieldName, string promptMessage, Func<string, (bool, string)> validation, bool optional = false)
        {
            bool valid = false; 
            string input = "";
            while (!valid)
            {
                Console.WriteLine(promptMessage);
                Console.Write("> ");
                input = Console.ReadLine().Trim();
        
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleUtils.ExitOnInputExit(input, $"Exiting {fieldName.ToLower()} process.");
                    return (true, input);
                }
        
                if (optional && string.IsNullOrEmpty(input))
                {
                    return (false, "");
                }
        
                var (isValid, errorMessage) = validation(input);
                
                if (!isValid)
                {
                    Console.WriteLine($"Invalid input: " + Colors.Colorize(errorMessage, Colors.Orange) + " Please try again.");
                }
                else
                {
                    valid = true;
                }
            }
        
            return (false, input);
        }
    }
}
