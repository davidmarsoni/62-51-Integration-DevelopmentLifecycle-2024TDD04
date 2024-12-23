using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;

namespace ConsoleApp.commands
{
    public abstract class BaseCommand : ICommand
    {
        private readonly Dictionary<string, ISubCommand> subCommands;
        private readonly string description;

        protected BaseCommand(string description, Dictionary<string, ISubCommand> subCommands)
        {
            this.description = description;
            this.subCommands = subCommands;
        }

        public void Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine(description.Split('-')[0].Trim() + " : " 
                    + Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }

            var cmd = arguments[0];
            if (subCommands.TryGetValue(cmd, out ISubCommand? subCommand))
                subCommand.Execute(arguments.Skip(1).ToArray());
            else
                Console.WriteLine(description.Split('-')[0].Trim() + " : " 
                    + Colors.Colorize("Command not found", Colors.Red));
        }

        public string GetDescription() => description;
        public string GetSubCommands()
        {
            string result = "Sub commands : \n";
            foreach (var sc in subCommands)
                result += sc.Value.GetDescription() + "\n";
            return result;
        }
    }
}
