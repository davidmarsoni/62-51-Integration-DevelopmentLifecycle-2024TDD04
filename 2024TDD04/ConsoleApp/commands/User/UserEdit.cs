using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class UserEdit : ISubCommand
    {
        private readonly UserService userService;
        public static string CommandName => "edit";

        public UserEdit(UserService userService)
        {
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Edit User\" process...");
            int userId = InputHelper.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
            if (userId == -1) return;

            UserDTO userDTO = userService.GetUserById(userId).Result;
            if (userDTO == null) { 
                Console.WriteLine(Colors.Colorize("User not found. Exiting ...", Colors.Red));
                return;
            }

            string userInput = InputHelper.PromptForString("Username", "Enter a new Username for the user. (Press Enter to skip)");
            if (userInput == null) return;

            if (string.IsNullOrEmpty(userInput)) {
                Console.WriteLine("No changes made. Exiting user editing.");
                return;
            }

            Boolean userExist = userService.GetUsernameExist(userInput).Result;
            if (userExist != null && userExist) {
                Console.WriteLine("The username already exists. Exiting user editing.");
                return;
            }

            userDTO.Username = userInput;
            if (userService.UpdateUser(userDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully edited the user.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when editing the user in the DB...", Colors.Red));
        }

        public string GetDescription() => $"{CommandName} : Edit user details.";
    }
}
