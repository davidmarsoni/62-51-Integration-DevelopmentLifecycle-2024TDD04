using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("List existing users");
            IEnumerable<UserDTO>? userDTOs = userService.GetAllUsers().Result;
            EntityCommandUtils.ListEntities(userDTOs, "User", user => $"{user.Id} - {user.Username}");
        }

        public string GetDescription() => $"{CommandName} : List existing users.";
    }
}
