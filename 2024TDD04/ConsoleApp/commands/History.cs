using ConsoleApp.commands;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using Microsoft.IdentityModel.Tokens;
using MVC.Services;
using MVC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.commands
{
    public class History : ICommand
    {
        private readonly IAccessLogService accessLogService;

        public History(HttpClient httpClient, string baseURL, bool debug)
        {
            accessLogService = new AccessLogService(httpClient, baseURL, debug);

        }

        public void Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine("History : " +  Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }

            switch (arguments[0])
            {
                case "list":
                    ListHistory();
                    break;
                default:
                    Console.WriteLine("History : " + Colors.Colorize("Command not found", Colors.Red));
                    break;
            }
        }

        public void ListHistory()
        {
            Console.WriteLine("Enter the log number (or type 'exit' to skip):");
            string logNumberInput = ConsoleManager.WaitInput(ConsoleUtils.EmptyOrIntValidation, "Enter the log number (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(logNumberInput, "Exiting history listing."))
                return;
            int? logNumber = string.IsNullOrEmpty(logNumberInput) ? (int?)null : int.Parse(logNumberInput);

            Console.WriteLine("Enter the offset (or type 'exit' to skip):");
            string offsetInput = ConsoleManager.WaitInput(ConsoleUtils.EmptyOrIntValidation, "Enter the offset (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(offsetInput, "Exiting history listing."))
                return;
            int? offset = string.IsNullOrEmpty(offsetInput) ? (int?)null : int.Parse(offsetInput);

            Console.WriteLine("Enter the order (or type 'exit' to skip):");
            string order = ConsoleManager.WaitInput(ConsoleUtils.EmptyOrIntValidation, "Enter the order (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(order, "Exiting history listing."))
                return;

            Console.WriteLine("Fetching access history logs...");
            var logs = accessLogService.GetAccessLog(logNumber, offset, order).Result;
            
            // If there are no logs
            if (logs.IsNullOrEmpty())
            {
                Console.WriteLine("History : " + Colors.Colorize("No logs found", Colors.Orange));
                return;
            }

            // Print the logs
            EntityCommandUtils.ListEntities(logs, "Log", log =>
            {
                return $"{log.Id} - {log.RoomId} - {log.UserId} - {log.TimeStamp}";
            }, 
                new List<string> { 
                    "Access logs : " + Colors.Colorize(logs.Count() + " logs found", Colors.Green),
                    "ID - Room ID - User ID - Timestamp" 
                    }
            );
        }

        public string GetDescription()
        {
            return "history - Manages the history.";
        }

        public string GetSubCommands()
        {
            return "history list - Lists the history.";
        }
    }
}
