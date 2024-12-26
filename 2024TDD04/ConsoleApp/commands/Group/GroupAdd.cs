using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

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
            Title("Add a new group");
            var (exit, nameInput) = InputUtils.PromptForString("Name",
                "Enter the " + Colors.Colorize("Name", Colors.Yellow) + " of the group. (or type 'exit')",
                (input) => GroupUtils.ValidateName(groupService, input));
            if (exit) return;
            var (exit2, acronymInput) = InputUtils.PromptForString("Acronym",
                "Enter an optional " + Colors.Colorize("Acronym", Colors.Yellow) + " for the group. (or type 'exit')",
                (input) => GroupUtils.ValidateAcronym(groupService, input),
                true);
            if (exit2) return;

            GroupDTO groupDTO = new () 
            {
                Name = nameInput,
                Acronym = string.IsNullOrEmpty(acronymInput) ? null : acronymInput
            };
            if (groupService.CreateGroup(groupDTO).Result != null)
                Success("Successfully added the group.");
            else
                Error("An error occurred when adding the group to the DB...");
        }

        public string GetDescription() => $"{CommandName} : Add a new group.";


    }
}
