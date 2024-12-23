using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class UserDelete : ISubCommand
    {
        private readonly UserService userService;
        public static string CommandName => "delete";

        public UserDelete(UserService userService)
        {
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Delete User\" process...");
            int userId = InputHelper.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
            if (userId == -1) return;

            UserDTO userDTO = userService.GetUserById(userId).Result;
            if (userDTO.IsDeleted)
            {
                Console.WriteLine(Colors.Colorize("User not found. Exiting ...", Colors.Red));
                return;
            }

            Console.WriteLine(Colors.Colorize("Deleting the user...", Colors.Red));
            EntityCommandUtils.ConfirmAndDeleteEntity(userId, userService.DeleteUser, "User");
        }

        public string GetDescription() => $"{CommandName} : Delete a user.";
    }
}
