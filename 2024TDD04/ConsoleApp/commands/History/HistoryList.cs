using ConsoleApp.commands.interfaces;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using MVC.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using ConsoleApp.console;

namespace ConsoleApp.commands.History
{
    public class HistoryList : ISubCommand
    {
        private readonly IAccessLogService accessLogService;
        public static string CommandName => "list";

        public HistoryList(IAccessLogService service)
        {
            accessLogService = service;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"List History\" process...");
        
            // ask the user to enter the log number
            Console.WriteLine("Enter the " + Colors.Colorize("Log number", Colors.Yellow) + " to list history.");
            string logNumberInput = ConsoleManager.WaitInput(
                ConsoleUtils.IsIntValidation,
                "To list history, input the " + Colors.Colorize("Log number", Colors.Yellow) + ". (or type 'exit')"
            ).ToLower();
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(logNumberInput, "Exiting list history."))
                return;
        
            // ask the user to enter the offset
            Console.WriteLine("Enter the " + Colors.Colorize("Offset", Colors.Yellow) + " to list history.");
            string offsetInput = ConsoleManager.WaitInput(
                ConsoleUtils.IsIntValidation,
                "To list history, input the " + Colors.Colorize("Offset", Colors.Yellow) + ". (or type 'exit')"
            ).ToLower();
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(offsetInput, "Exiting list history."))
                return;
        
            // ask the user to enter the order
            Console.WriteLine("Enter the " + Colors.Colorize("Order", Colors.Yellow) + " to list history.");
            string order = ConsoleManager.WaitInput(
                input => !string.IsNullOrEmpty(input),
                "To list history, input the " + Colors.Colorize("Order", Colors.Yellow) + ". (or type 'exit')"
            ).ToLower();
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(order, "Exiting list history."))
                return;
        
            // parse the log number and offset
            int? logNumber = string.IsNullOrEmpty(logNumberInput) ? null : int.Parse(logNumberInput);
            int? offset = string.IsNullOrEmpty(offsetInput) ? null : int.Parse(offsetInput);
        
            Console.WriteLine("Fetching access history logs...");
            var logs = accessLogService.GetAccessLog(logNumber, offset, order).Result;
            if (logs.IsNullOrEmpty())
            {
                Console.WriteLine("History : " + Colors.Colorize("No logs found", Colors.Orange));
                return;
            }
        
            EntityCommandUtils.ListEntities(logs, "Log", log =>
            {
                return $"{log.Id} - {log.RoomId} - {log.UserId} - {log.TimeStamp}";
            },
            new List<string> {
                $"Access logs : {Colors.Colorize($"{logs.Count()} logs found", Colors.Green)}",
                "ID - Room ID - User ID - Timestamp"
            });
        }

        public string GetDescription() => $"{CommandName} - Lists the history.";
    }
}
