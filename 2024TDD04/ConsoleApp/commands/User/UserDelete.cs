using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("Delete a user");
            int userId = InputUtils.PromptForInt("User Id", "Enter the User ID (or type 'exit')");
            if (userId == -1) return;

            UserDTO? userDTO = userService.GetUserById(userId).Result;
            if (userDTO == null || userDTO.IsDeleted)
            {
                Error("User not found. Exiting ...");
                return;
            }
            Warning("Deleting the user...");
            EntityCommandUtils.ConfirmAndDeleteEntity(userId, userService.DeleteUser, "User"); 
        }

        public string GetDescription() => $"{CommandName} : Delete a user.";
    }
}
