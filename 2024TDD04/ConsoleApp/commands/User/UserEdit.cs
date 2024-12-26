using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("Edit a user");
            int userId = InputUtils.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
            if (userId == -1) return;

            UserDTO? userDTO = userService.GetUserById(userId).Result;
            if (userDTO == null || userDTO.IsDeleted)
            {
                Console.WriteLine(Colors.Colorize("User not found. Exiting ...", Colors.Red));
                return;
            }

            string prompt = $"Enter the new {Colors.Colorize($"Username ({userDTO.Username})", Colors.Yellow)} of the user. (or type 'exit')";
            var (exit, input) = InputUtils.PromptForString("Username", prompt, (u) => UserUtils.ValidateUsername(userService, u));
            if (exit) return;

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No changes made. Exiting user editing.");
                return;
            }

            userDTO.Username = input;
            if (userService.UpdateUser(userDTO).Result)
                Success("Successfully edited the user.");
            else
                Error("An error occurred when editing the user in the DB...");
        }

        public string GetDescription() => $"{CommandName} : Edit user details.";
    }
}
