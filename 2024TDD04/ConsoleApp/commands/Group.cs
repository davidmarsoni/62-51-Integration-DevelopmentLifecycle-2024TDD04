using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands
{
    public class Group : ICommand
    {
        private GroupService groupService;
        public Group(HttpClient httpClient, string baseURL)
        {
            groupService = new GroupService(httpClient, baseURL);
        }

        public void Execute(String[] arguments)
        {
            if (arguments.Length == 0)
            {
                Console.WriteLine("Group : " + Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }
            switch (arguments[0])
            { 
                case "add":
                    AddGroup();
                    break;
                case "delete":
                    DeleteGroup();
                    break;
                case "list":
                    ListGroups();
                    break;
                case "edit":
                    EditGroup();
                    break;
                default:
                    Console.WriteLine("Group : " + Colors.Colorize("Command not found", Colors.Red));
                    break;
            }
        }

        public string GetDescription()
        {
            return "group - Manage groups.";
        }

        public string GetSubCommands()
        {
            return "add, delete, list, edit";
        }

        // methods
        private void ListGroups()
        {
            IEnumerable<GroupDTO>? groupDTOs = groupService.GetAllGroups().Result;
            EntityCommandUtils.ListEntities(groupDTOs, "Group", group =>
            {
                string acronym = string.IsNullOrEmpty(group.Acronym) ? "" : $" ({group.Acronym})";
                return $"{group.Id} - {group.Name}{acronym}";
            });
        }

        private void AddGroup()
        {
            Console.WriteLine("Beginning the \"Add Group\" process...");
            Console.WriteLine("Enter a " + Colors.Colorize("Name", Colors.Yellow) + " to create a group.");
            String nameInput = ConsoleManager.WaitInput(ValidateGroupNameIsUnique, "Enter the group name (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting group creation."))
                return;
            Console.WriteLine("Enter an optional " + Colors.Colorize("Acronym", Colors.Yellow) + " for the group.");
            String acronymInput = ConsoleManager.WaitInput(ValidateGroupAcronymIsUnique, "(Press enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(acronymInput, "Exiting group creation."))
                return;
            GroupDTO groupDTO = new GroupDTO
            {
                Name = nameInput,
                Acronym = string.IsNullOrEmpty(acronymInput) ? null : acronymInput,
                IsDeleted = false
            };
            if (groupService.CreateGroup(groupDTO).Result != null)
                Console.WriteLine(Colors.Colorize("Successfully added the group.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when adding the group to the DB...", Colors.Red));
        }

        private void EditGroup()
        {
            Console.WriteLine("Beginning the \"Edit Group\" process...");
            Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to edit a group.");
            String groupIdInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To edit a group, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')").ToLower();
            if (ConsoleUtils.ExitOnInputExit(groupIdInput, "Exiting group editing."))
                return;
            int groupId;
            try
            {
                groupId = int.Parse(groupIdInput);
            }
            catch
            {
                Console.WriteLine(Colors.Colorize("Invalid Group Id. Exiting...", Colors.Red));
                return;
            }
            GroupDTO groupDTO = groupService.GetGroupById(groupId).Result;
            if (groupDTO == null)
            {
                Console.WriteLine(Colors.Colorize("Group not found. Exiting...", Colors.Red));
                return;
            }
            Console.WriteLine("Enter a new " + Colors.Colorize("Name", Colors.Yellow) + " for the group. (Press Enter to skip)");
            String nameInput = ConsoleManager.WaitInput(ValidateGroupNameIsUniqueForEdit, "(Press Enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting group editing."))
                return;

            Console.WriteLine("Enter a new " + Colors.Colorize("Acronym", Colors.Yellow) + " for the group. (Press Enter to skip)");
            String acronymInput = ConsoleManager.WaitInput(ValidateGroupAcronymIsUnique, "(Press Enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(acronymInput, "Exiting group editing."))
                return;

            if (string.IsNullOrEmpty(nameInput) && string.IsNullOrEmpty(acronymInput))
            {
                Console.WriteLine("No changes made. Exiting group editing.");
                return;
            }

            if (!string.IsNullOrEmpty(nameInput))
                groupDTO.Name = nameInput;

            if (!string.IsNullOrEmpty(acronymInput))
                groupDTO.Acronym = acronymInput;

            if (groupService.UpdateGroup(groupDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully edited the group.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when editing the group in the DB...", Colors.Red));
        }

        private void DeleteGroup()
        {
            Console.WriteLine("Beginning the \"Delete Group\" process...");
            Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to delete a group.");
            String groupIdInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To delete a group, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')").ToLower();
            if (ConsoleUtils.ExitOnInputExit(groupIdInput, "Exiting group deletion."))
                return;
            int groupId;
            try
            {
                groupId = int.Parse(groupIdInput);
            }
            catch
            {
                Console.WriteLine("An error occurred when parsing the groupId. Exiting...", Colors.Red);
                return;
            }
            EntityCommandUtils.ConfirmAndDeleteEntity(groupId, groupService.DeleteGroup, "Group");
        }

        private string GroupInputValidName()
        {
            String name = "";
            while (true)
            {
                name = ConsoleManager.WaitInput(GroupNameValidation,
                    "Enter the " + Colors.Colorize("Group Name", Colors.Yellow) + ". (or type 'exit')");
                if (ConsoleUtils.ExitOnInputExit(name, ""))
                    break;
                if (!string.IsNullOrEmpty(name))
                    break;
            }
            return name;
        }

        // validation methods

        private static Boolean GroupNameValidation(string input)
        {
            return !String.IsNullOrEmpty(input) || input.ToLower() == "exit";
        }

        private bool ValidateGroupNameIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            if (input.ToLower() == "exit")
                return true;
            if (groupService.GroupNameExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Group name already exists. Please choose a different name.", Colors.Red));
                return false;
            }
            return true;
        }

        private bool ValidateGroupNameIsUniqueForEdit(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                return true;
            if (groupService.GroupNameExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Group name already exists. Please choose a different name.", Colors.Red));
                return false;
            }
            return true;
        }

        private bool ValidateGroupAcronymIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                return true;
            if (groupService.GroupAcronymExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Group acronym already exists. Please choose a different acronym.", Colors.Red));
                return false;
            }
            return true;
        }
    }
}
