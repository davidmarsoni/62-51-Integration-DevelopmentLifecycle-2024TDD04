using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
namespace ConsoleApp.commands.Group
{
    public class GroupEdit : ISubCommand
    {
        private readonly GroupService groupService;
        public static string CommandName => "edit";

        public GroupEdit(GroupService groupService)
        {
            this.groupService = groupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Edit Group\" process...");
            Console.WriteLine("Enter the " + Colors.Colorize("Group Id", Colors.Yellow) + " to edit a group.");
            int groupId = InputHelper.PromptForInt("Group Id", "To edit a group, input the ID. (or type 'exit')");
            if (groupId == -1) return;

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

        public string GetDescription() => $"{CommandName} : Edit a group.";
        
    }
}
