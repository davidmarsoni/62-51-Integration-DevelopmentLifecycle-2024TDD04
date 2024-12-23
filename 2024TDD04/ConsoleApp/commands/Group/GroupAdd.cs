using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.helpers;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.Group
{
    public class GroupAdd : ISubCommand
    {
        private readonly GroupService groupService;
        public static string CommandName => "add";

        public GroupAdd(GroupService groupService)
        {
            this.groupService = groupService;
        }

        public void Execute(string[] arguments)
        {
            Console.WriteLine("Beginning the \"Add Group\" process...");
            Console.WriteLine("Enter a " + Colors.Colorize("Name", Colors.Yellow) + " to create a group.");
            string nameInput = InputHelper.PromptForString("Name", "Enter the group name (or type 'exit')");
            if (nameInput == null) return;
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

        public string GetDescription() => $"Usage: {CommandName} <name> [acronym]";


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
