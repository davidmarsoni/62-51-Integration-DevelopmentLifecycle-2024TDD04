using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

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
            Console.WriteLine("Beginning the \"Add User\" process...");
            Console.WriteLine("Enter a " + Colors.Colorize("Username", Colors.Yellow) + " to create a user.");
            String userInput = UserInputValidUsername();
            if (ConsoleUtils.ExitOnInputExit(userInput, "Exiting user creation."))
                return;
            UserDTO userDTO = new UserDTO();
            userDTO.Username = userInput.Split(" ")[0];
            userDTO.IsDeleted = false;
            if (userService.CreateUser(userDTO).Result != null)
                Console.WriteLine(Colors.Colorize("Successfully added the user.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occured when adding the user to the DB...", Colors.Red));
        }

        private string UserInputValidUsername(){
            Boolean valid = false;
            String username = "";
            while (!valid)
            {
                username = ConsoleManager.WaitInput(
                    AddUserValidation,
                    "To add/edit a user, input the " + Colors.Colorize("Username", Colors.Yellow) + ". (or type 'exit')"
                    );
                if (username.ToLower() == "exit")
                {
                    return "exit";
                }
                Boolean userExist = userService.GetUsernameExist(username).Result;
                if (userExist != null && userExist)
                {
                    Console.WriteLine("The username already exists. Please input a new username.");
                }
                else
                {
                    valid = true;
                }
            }
            return username;
        }

        private static Boolean AddUserValidation(string input) {
            return !String.IsNullOrEmpty(input) && input.Split(" ").Length > 0 || input.Split(" ")[0].ToLower() == "exit";
        }

        public string GetDescription() => $"{CommandName} : Add a new user.";
    }
}
