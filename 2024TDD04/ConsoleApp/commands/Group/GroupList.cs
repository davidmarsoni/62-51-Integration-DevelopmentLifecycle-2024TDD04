using ConsoleApp.commands.interfaces;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using static ConsoleApp.utils.ConsoleUtils;

namespace ConsoleApp.commands.Group
{
    public class GroupList : ISubCommand
    {
        private readonly GroupService groupService;
        public static string CommandName => "list";

        public GroupList(GroupService groupService)
        {
            this.groupService = groupService;
        }

        public void Execute(string[] arguments)
        {
            Title("List existing groups");
            IEnumerable<GroupDTO>? groupDTOs = groupService.GetAllGroups().Result;
            EntityCommandUtils.ListEntities(groupDTOs, "Group", group =>
            {
                string acronym = string.IsNullOrEmpty(group.Acronym) ? "" : $" ({group.Acronym})";
                return $"{group.Id} - {group.Name}{acronym}";
            });
        }

        public string GetDescription() => $"{CommandName} : List all groups.";
    }
}
