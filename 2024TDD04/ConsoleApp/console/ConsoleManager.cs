using ConsoleApp.commands;
using ConsoleApp.commands.Access;
using ConsoleApp.commands.Group;
using ConsoleApp.commands.History;
using ConsoleApp.commands.interfaces;
using ConsoleApp.commands.Room;
using ConsoleApp.commands.Test;
using ConsoleApp.commands.User;
using ConsoleApp.utils;

namespace ConsoleApp.console
{
    public class ConsoleManager
    {
        // dictionary of commands
        private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        public ConsoleManager(HttpClient httpClient, String baseURL, Boolean debug)
        {
            commands.Add(Room.CommandName, new Room(httpClient, baseURL, debug));
            commands.Add(History.CommandName, new History(httpClient, baseURL, debug));
            commands.Add(Help.CommandName, new Help(commands));
            commands.Add(User.CommandName, new User(httpClient, baseURL, debug));
            commands.Add(Group.CommandName, new Group(httpClient, baseURL, debug));
            commands.Add(Access.CommandName, new Access(httpClient, baseURL, debug));
            commands.Add(Test.CommandName, new Test(httpClient, baseURL, debug));
        }

        public void Launch()
        {
            // show the hello message
            HelloMessage();

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

        private void HelloMessage()
        {
            Console.WriteLine();
            Console.WriteLine(@"██████╗  ██████╗  ██████╗ ███╗   ███╗     █████╗  ██████╗ ██████╗███████╗███████╗███████╗
██╔══██╗██╔═══██╗██╔═══██╗████╗ ████║    ██╔══██╗██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝
██████╔╝██║   ██║██║   ██║██╔████╔██║    ███████║██║     ██║     █████╗  ███████╗███████╗
██╔══██╗██║   ██║██║   ██║██║╚██╔╝██║    ██╔══██║██║     ██║     ██╔══╝  ╚════██║╚════██║
██║  ██║╚██████╔╝╚██████╔╝██║ ╚═╝ ██║    ██║  ██║╚██████╗╚██████╗███████╗███████║███████║
╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝     ╚═╝    ╚═╝  ╚═╝ ╚═════╝╚═════╝╚══════╝╚══════╝╚══════╝");
            Console.WriteLine();
            Console.WriteLine(Colors.Colorize("Type 'help' to see the list of commands", Colors.Green));
        }
    }
}
