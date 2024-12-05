using ConsoleApp.commands;
using ConsoleApp.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.console
{
    public class ConsoleManager
    {
        // dictionary of commands
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public ConsoleManager(HttpClient httpClient, String baseURL)
        {
            commands.Add("room", new Room());
            commands.Add("history", new History());
            commands.Add("help", new Help(commands));
            commands.Add("user", new User(httpClient, baseURL));
            commands.Add("group", new Group(httpClient, baseURL));
            commands.Add("access", new Access());
        }

        public void Launch()
        {
            while (true)
            {
                // print '>' character
                Console.Write("> ");

                // wait for user input
                String input = Console.ReadLine();

                // if the input is not empty
                if (!String.IsNullOrEmpty(input))
                {
                    // trim the input
                    input = input.Trim();
                    // split the arguments
                    String[] arguments = input.Split(' ');
                    // put all the arguments to lower case
                    arguments = arguments.Select(x => x.ToLower()).ToArray();

                    // search in the dictionary of commands
                    if (commands.ContainsKey(arguments[0]))
                    {
                        // execute the command
                        commands[arguments[0]].Execute(arguments.Skip(1).ToArray());
                    }
                    else
                    {
                        // if the command is not found, but is "exit", then exit the program
                        if (arguments[0] == "exit")
                        {
                            Console.WriteLine("Exiting...");
                            break;
                        }
                        // if the command is still not found, print the command not found
                        else
                        {
                            Console.WriteLine(Colors.Colorize("Command not found", Colors.Red));
                        }
                    }
                }
            }
        }

        public static string WaitInput(Func<string, Boolean> validation, string errorMessage) {
            string output = "";
            Boolean isValid = false;
            while (!isValid) {
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

        public void ShowHelp()
        {
            foreach (var command in commands)
            {
                Console.WriteLine(command.Key + " - " + command.Value.GetDescription());
            }
        }
    }
}
