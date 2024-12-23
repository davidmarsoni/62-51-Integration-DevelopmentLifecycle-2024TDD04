using MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using MVC.Services.Interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;

namespace ConsoleApp.commands
{
    public class Test : ICommand
    {
        private readonly ITestService _testService;

        public Test(HttpClient httpClient, string baseURL, bool debug)
        {
            _testService = new TestService(httpClient, baseURL, debug);
        }

        public void Execute(string[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine("Test : No arguments provided");
                return;
            }
            switch (arguments[0])
            {
                case "access":
                    TestAccess();
                    break;
                default:
                    Console.WriteLine("Test : Command not found");
                    break;
            }
        }

        public string GetDescription()
        {
            return "test - Commands for testing purposes.";
        }

        public string GetSubCommands()
        {
            return "access";
        }

        private void TestAccess()
        {
            Console.WriteLine("Beginning the \"Test Access\" process...");

            // ask the user to enter the room ID
            Console.WriteLine("Enter the " + Colors.Colorize("Room Id", Colors.Yellow) + " to test access.");
            string roomIdInput = ConsoleManager.WaitInput(
                ConsoleUtils.IsIntValidation,
                "To test access, input the " + Colors.Colorize("Room Id", Colors.Yellow) + ". (or type 'exit')"
            ).ToLower();
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting test access."))
                return;

            // ask the user to enter the user ID
            Console.WriteLine("Enter the " + Colors.Colorize("User Id", Colors.Yellow) + " to test access.");
            string userIdInput = ConsoleManager.WaitInput(
                ConsoleUtils.IsIntValidation,
                "To test access, input the " + Colors.Colorize("User Id", Colors.Yellow) + ". (or type 'exit')"
            ).ToLower();
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(userIdInput, "Exiting test access."))
                return;

            // parse the IDs
            int roomId;
            int userId;
            try
            {
                roomId = int.Parse(roomIdInput);
                userId = int.Parse(userIdInput);
            }
            catch
            {
                // if the IDs are not parsable, this means something went wrong with the validation
                Console.WriteLine(Colors.Colorize("An error occurred when parsing the IDs. Exiting...", Colors.Red));
                return;
            }

            // create the DTO object
            RoomAccessDTO roomAccessDTO = new RoomAccessDTO
            {
                RoomId = roomId,
                UserId = userId
            };

            // try to access the room
            Console.WriteLine("Trying to access the room...");
            var result = _testService.TestAccessAsync(roomAccessDTO).Result;
            if (result)
            {
                Console.WriteLine(Colors.Colorize("Access successful.", Colors.Green));
            }
            else
            {
                Console.WriteLine(Colors.Colorize("Access denied.", Colors.Red));
            }
        }
    }
}
