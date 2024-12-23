using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class UserList : ISubCommand
    {
        private readonly UserService userService;
        public static string CommandName => "list";

        public UserList(UserService userService)
        {
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"List Users\" process...");
            IEnumerable<UserDTO>? userDTOs = userService.GetAllUsers().Result;
            EntityCommandUtils.ListEntities(userDTOs, "User", user => $"{user.Id} - {user.Username}");
        }

        public string GetDescription() => $"{CommandName} : List existing users.";
    }
}
