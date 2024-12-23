using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;

namespace ConsoleApp.commands
{
    public class Help : ICommand
    {
        // dictionary of commands
        private Dictionary<string, ICommand> commands;

        public Help(Dictionary<string, ICommand> commands)
        {
            this.commands = commands;
        }

        public void Execute(string[] arguments)
        {
            // if there's an argument, find the subcommands of the command
            if (arguments.Length > 0)
            {
                // get the command name
                string commandName = arguments[0];

                // check if the command exists
                if (commands.ContainsKey(commandName))
                {
                    // get the command
                    ICommand command = commands[commandName];

                    // print the description
                    Console.WriteLine(command.GetDescription());
                    // print the subcommands
                    Console.WriteLine(command.GetSubCommands());
                }
                else
                {
                    Console.WriteLine("Help: " + Colors.Colorize(" Command not found.", Colors.Red));
                }

                return;
            }
            // loop through the commands and print the description
            foreach (var command in commands)
            {
                Console.WriteLine(command.Value.GetDescription());
            }
        }

        public string GetDescription() => "help : Displays the list of available commands.";

        public string GetSubCommands() => "help [command name] : Displays the subcommands of a command.";
        
    }
}
