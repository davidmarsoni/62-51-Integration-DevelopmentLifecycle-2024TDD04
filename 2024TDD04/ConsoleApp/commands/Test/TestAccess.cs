using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using ConsoleApp.helpers;
using DTO;
using MVC.Services.Interfaces;

namespace ConsoleApp.commands.Test
{
    public class TestAccess : ISubCommand
    {
        private readonly ITestService _testService;
        public static string CommandName => "access";

        public TestAccess(ITestService testService)
        {
            _testService = testService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Test Access\" process...");
            int roomId = InputHelper.PromptForInt("Room Id", "To test access, input the Room Id (or type 'exit')");
            if (roomId == -1) return;

            int userId = InputHelper.PromptForInt("User Id", "To test access, input the User Id (or type 'exit')");
            if (userId == -1) return;

            var dto = new RoomAccessDTO { RoomId = roomId, UserId = userId };
            Console.WriteLine("Trying to access the room...");
            bool result = _testService.TestAccessAsync(dto).Result;
            Console.WriteLine(result
                ? Colors.Colorize("Access successful.", Colors.Green)
                : Colors.Colorize("Access denied.", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} - Test room access.";
    }
}
