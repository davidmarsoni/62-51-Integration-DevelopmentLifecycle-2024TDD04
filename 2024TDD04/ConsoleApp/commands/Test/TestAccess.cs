using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services.Interfaces;
using static ConsoleApp.utils.ConsoleUtils;

namespace ConsoleApp.commands.Test
{
    public class TestAccess : ISubCommand
    {
        private readonly ITestService testService;
        private readonly IAccessLogService accessLogService;

        public static string CommandName => "access";

        public TestAccess(ITestService testService, IAccessLogService accessLogService)
        {
            this.testService = testService;
            this.accessLogService = accessLogService;
        }

        public void Execute(string[] arguments)
        {
            Title("Test Access");
            int roomId = InputUtils.PromptForInt("Room Id", "To test access, input the Room Id (or type 'exit')");
            if (roomId == -1) return;
        
            int userId = InputUtils.PromptForInt("User Id", "To test access, input the User Id (or type 'exit')");
            if (userId == -1) return;
                    
            Console.WriteLine("Trying to access the room...");
            RoomAccessDTO? result = testService.TestAccessRoom(roomId, userId).Result;
        
            if (result != null)
                Success("Access successful.");
            else
                Error("Access denied.");
        }
        public string GetDescription() => $"{CommandName} - Test room access.";
    }
}
