using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;
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
            Title("Edit a group");
            var idInput = InputUtils.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
            GroupDTO? groupDTO = groupService.GetGroupById(idInput).Result;
            if (groupDTO == null)
            {
                Error("The group with the given ID does not exist.");
                return;
            }
            var (exit2, nameInput) = InputUtils.PromptForString("Name",
                "Enter the new " + Colors.Colorize($"Name ({groupDTO.Name})", Colors.Yellow) + " of the group leaving blank will keep the same name (or type 'exit')",
                (input) => GroupUtils.ValidateName(groupService, input),
                true);
            if (exit2) return;
            var (exit3, acronymInput) = InputUtils.PromptForString("Acronym",
                "Enter an new optional " + Colors.Colorize($"Acronym ({groupDTO.Acronym})", Colors.Yellow) + " for the group leaving blank will remove the acronym (or type 'exit')",
                (input) => GroupUtils.ValidateAcronym(groupService, input),
                true);
            if (exit3) return;

            if(nameInput != "" || nameInput != null)
                groupDTO.Name = nameInput;
                
            groupDTO.Acronym = acronymInput;
            if (groupService.UpdateGroup(groupDTO).Result)
                Success("Successfully edited the group.");
            else
                Error("An error occurred when editing the group in the DB...");
        }

        public string GetDescription() => $"{CommandName} : Edit a group.";
        
    }
}
