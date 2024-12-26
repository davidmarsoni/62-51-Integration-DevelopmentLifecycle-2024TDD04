using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using MVC.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("List History");
    
            // ask the user to enter the log number
            var logNumberInput = InputUtils.PromptForInt("Log number", "To list history, input the Log number. (or type 'exit')");
            // check if the user wants to exit
            if (logNumberInput == -1)
                return;
        
            // ask the user to enter the offset
            var offsetInput = InputUtils.PromptForInt("Offset", "To list history, input the Offset. (or type 'exit')");
           
            // check if the user wants to exit
            if (offsetInput == -1)
                return;
        
            // ask the user to enter the order
            var (exit, order) = InputUtils.PromptForString("Order", "To list history, input the Order. (asc/desc) (or type 'exit')", OrderVerify);
           
            // check if the user wants to exit
            if (exit)
                return;
        
            // parse the log number and offset
            int logNumber = logNumberInput;
            int offset = offsetInput;
        
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

        public (bool, string) OrderVerify(string input) {
            if (!input.Equals("asc", StringComparison.OrdinalIgnoreCase) &&
                !input.Equals("desc", StringComparison.OrdinalIgnoreCase))
                return (false, "History : " + Colors.Colorize("Invalid order. Please type 'asc' or 'desc'", Colors.Orange));
            return (true, null);
        }

        public string GetDescription() => $"{CommandName} - Lists the history.";
    }
}
