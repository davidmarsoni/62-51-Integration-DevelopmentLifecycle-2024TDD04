using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands
{
    public class User : ICommand
    {
        private UserService userService;
        public User(HttpClient httpClient, string baseURL)
        {
            userService = new UserService(httpClient, baseURL);
        }

        public void Execute(String[] arguments)
        {
            // check if there are no arguments
            if (arguments.Length == 0)
            {
                Console.WriteLine("User : " + Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }
            // evaluate the arguments and perform the appropriate action
            switch (arguments[0])
            { 
                case "add":
                    AddUser();
                    break;
                case "delete":
                    DeleteUser();
                    break;
                case "list":
                    ListUsers();
                    break;
                case "edit":
                    EditUser();
                    break;
                default:
                    // print the command not found
                    Console.WriteLine("User : " + Colors.Colorize("Command not found", Colors.Red));
                    break;
            }
        }

        public string GetDescription()
        {
            return "user - Manage users.";
        }

        public string GetSubCommands()
        {
            return "add, delete, list, edit";
        }

        // methods
        private void ListUsers() {
            IEnumerable<UserDTO>? userDTOs = userService.GetAllUsers().Result;
            EntityCommandUtils.ListEntities(userDTOs, "User", user => $"{user.Id} - {user.Username}");
        }

        private void AddUser() {
            // adding a user
            Console.WriteLine("Beginning the \"Add User\" process...");
            // ask the user to enter a username
            Console.WriteLine("Enter a " + Colors.Colorize("Username", Colors.Yellow) + " to create a user.");
            String userInput = UserInputValidUsername();
            // check if the user want to exit
            if (ConsoleUtils.ExitOnInputExit(userInput, "Exiting user creation."))
                return;
            // make the DTO object
            UserDTO userDTO = new UserDTO();
            userDTO.Username = userInput.Split(" ")[0];
            userDTO.IsDeleted = false;
            // send the new user
            if (userService.CreateUser(userDTO).Result != null)
                Console.WriteLine(Colors.Colorize("Successfully added the user.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occured when adding the user to the DB...", Colors.Red));
        }

        public void EditUser() {
            // editing a user
            Console.WriteLine("Beginning the \"Edit User\" process...");
            // ask the user to enter the username
            Console.WriteLine("Enter the " + Colors.Colorize("User Id", Colors.Yellow) + " to edit a user.");
            String userInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To edit a user, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')"
                ).ToLower();
            // check if the user want to exit
            if (ConsoleUtils.ExitOnInputExit(userInput, "Exiting user editing."))
                return;
            // check if the user exist
            int userId;
            try 
            {
                userId = int.Parse(userInput);
            } 
            catch (Exception e)
            {
                Console.WriteLine("An error occured when parsing the userId. Exiting ...", Colors.Red);
                return;
            }
            // get user exists
            UserDTO userDTO = userService.GetUserById(userId).Result;
            if (userDTO == null) { 
                Console.WriteLine("User not found. Exiting ...", Colors.Red);
                return;
            }
            // ask for new username (press Enter to skip)
            Console.WriteLine("Enter a new " + Colors.Colorize("Username", Colors.Yellow) + " for the user. (Press Enter to skip)");
            userInput = ConsoleManager.WaitInput(ConsoleUtils.EmptyValidationMethod, "(Press Enter to skip)");
            // check if the user wants to exit
            if (ConsoleUtils.ExitOnInputExit(userInput, "Exiting user editing."))
                return;
            // if no changes were made, exit
            if (string.IsNullOrEmpty(userInput)) {
                Console.WriteLine("No changes made. Exiting user editing.");
                return;
            }
            // check if the username already exists
            Boolean userExist = userService.GetUsernameExist(userInput).Result;
            if (userExist != null && userExist) {
                Console.WriteLine("The username already exists. Exiting user editing.");
                return;
            }
            // update the username
            userDTO.Username = userInput;
            // send the updated user
            if (userService.UpdateUser(userDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully edited the user.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when editing the user in the DB...", Colors.Red));
        }

        private string UserInputValidUsername(){
            // while the username is not valid, or already exists, ask the user to input a new username
            Boolean valid = false;
            String username = "";
            while (!valid)
            {
                username = ConsoleManager.WaitInput(
                    AddUserValidation,
                    "To add/edit a user, input the " + Colors.Colorize("Username", Colors.Yellow) + ". (or type 'exit')"
                    );
                // check if the user want to exit
                if (username.ToLower() == "exit")
                {
                    return "exit";
                }
                // check if the username already exists
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

        private void DeleteUser() {
            // deleting a user
            Console.WriteLine("Beginning the \"Delete User\" process...");
            // ask the user to enther the id of the user to delete
            Console.WriteLine("Enter the " + Colors.Colorize("User Id", Colors.Yellow) + " to delete a user.");
            String userInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To delete a user, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')"
                ).ToLower();
            // check if the user want to exit
            if (ConsoleUtils.ExitOnInputExit(userInput, "Exiting user deletion."))
                return;
            // ask the user to confirm the deletion
            Console.WriteLine(Colors.Colorize("Are you sure you want to delete the user? (type 'y' to confirm)", Colors.Red));
            // write the '> ' prompt
            Console.Write("> ");
            // get the user input
            String confirm = Console.ReadLine().ToLower();
            // if the confirmation is not 'y', exit
            if (confirm != "y")
            {
                Console.WriteLine("Exiting user deletion.");
                return;
            }
            // inform the user that the user is being deleted
            Console.WriteLine(Colors.Colorize("Deleting the user...", Colors.Red));
            // delete the user
            // if the user is deleted, print a success message
            int userId;
            try
            {
                userId = int.Parse(userInput);
            }
            catch
            {
                Console.WriteLine("An error occurred when parsing the userId. Exiting...", Colors.Red);
                return;
            }
            EntityCommandUtils.ConfirmAndDeleteEntity(userId, userService.DeleteUser, "User");
        }

        // validation methods

        private static Boolean AddUserValidation(string input) {
            return !String.IsNullOrEmpty(input) && input.Split(" ").Length > 0 || input.Split(" ")[0].ToLower() == "exit";
        }
    }
}
