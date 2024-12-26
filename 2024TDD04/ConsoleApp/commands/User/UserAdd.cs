using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

namespace ConsoleApp.commands.User
{
    public class UserAdd : ISubCommand
    {
        private readonly UserService userService;
        public static string CommandName => "add";

        public UserAdd(UserService userService)
        {
            this.userService = userService;
        }

        public void Execute(string[] arguments)
        {
            Title("Add a new user");

            // ask for username 
            string prompt ="Enter the "+ Colors.Colorize("Username", Colors.Yellow) + " of the user no space allowed (or type 'exit')";
            var (exit, input) = InputUtils.PromptForString("Username", prompt, (input) => UserUtils.ValidateUsername(userService, input));
            if (exit) return;

            // create user in the database
            UserDTO userDTO = new UserDTO();
            userDTO.Username = input.Split(" ")[0];
            userDTO.IsDeleted = false;

            if (userService.CreateUser(userDTO).Result != null)
                Success("Successfully added the user.");
            else
                Error("An error occured when adding the user to the DB...");
        }

        public string GetDescription() => $"{CommandName} : Add a new user.";
    }
}
