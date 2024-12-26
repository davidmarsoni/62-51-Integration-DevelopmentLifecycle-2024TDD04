using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class RoomUtils
    {
        public static (bool, string) ValidateName(RoomService roomService, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Name cannot be empty.");
            if (name.Length < 3 || name.Length > 50)
                return (false, "Name must be between 3 and 50 characters long.");
            if (roomService.NameExists(name).Result)
                return (false, "The name already exists.Please choose a different name.");
            return (true, string.Empty);
        }

        public static (bool, string) ValidateAbbreviation(RoomService roomService, string abbreviation)
        {
            if (string.IsNullOrWhiteSpace(abbreviation))
                return (false, "Abbreviation cannot be empty.");
            if (abbreviation.Length < 3 || abbreviation.Length > 10)
                return (false, "Abbreviation must be between 3 and 10 characters long.");
            if (roomService.AbreviationExists(abbreviation).Result)
                return (false, "The abbreviation already exists. Please choose a different abbreviation.");
            return (true, string.Empty);
        }
    }
}