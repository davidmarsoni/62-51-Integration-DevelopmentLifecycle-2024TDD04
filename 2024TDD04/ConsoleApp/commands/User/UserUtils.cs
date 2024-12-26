using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class UserUtils
    {
        public static (bool, string) ValidateUsername(UserService userService, string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return (false, "Username cannot be empty.");
            if (username.Length < 3 || username.Length > 50)
                return (false, "Username must be between 3 and 50 characters long.");
            if (userService.UsernameExist(username).Result)
                return (false, "The username already exists. Please choose a different username.");
            return (true, string.Empty);
        }
    }
}