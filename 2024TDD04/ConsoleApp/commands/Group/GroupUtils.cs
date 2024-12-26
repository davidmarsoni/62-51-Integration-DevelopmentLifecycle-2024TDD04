using MVC.Services;

namespace ConsoleApp.commands.Group
{
    public class GroupUtils
    {
        public static (bool, string) ValidateName(GroupService groupService, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "Name cannot be empty.");
            if (name.Length < 3 || name.Length > 50)
                return (false, "Name must be between 3 and 50 characters long.");
            if (groupService.GroupNameExists(name).Result)
                return (false, "The name already exists. Please choose a different name.");

            return (true, string.Empty);
        }

        public static (bool, string) ValidateAcronym(GroupService groupService, string acronym)
        {
            if (string.IsNullOrWhiteSpace(acronym))
                return (false, "Acronym cannot be empty.");
            if (acronym.Length < 3 || acronym.Length > 10)
                return (false, "Acronym must be between 3 and 10 characters long.");
            if (groupService.GroupAcronymExists(acronym).Result)
                return (false, "The acronym already exists. Please choose a different acronym.");

            return (true, string.Empty);
        }
    }
}