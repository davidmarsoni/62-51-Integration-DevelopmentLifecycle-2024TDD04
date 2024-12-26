using ConsoleApp.commands.interfaces;
using MVC.Services;
using ConsoleApp.utils;
using DTO;
using static ConsoleApp.utils.ConsoleUtils;

namespace ConsoleApp.commands.Access
{
    public class AccessRevoke : ISubCommand
    {
        private readonly AccessService accessService;
        private readonly GroupService groupService;
        private readonly RoomService roomService;
        public static string CommandName => "revoke";

        public AccessRevoke(AccessService accessService, GroupService groupService, RoomService roomService)
        {
            this.accessService = accessService;
            this.groupService = groupService;
            this.roomService = roomService;
        }

        public void Execute(string[] arguments)
        {
            Title("Revoke Access");
            int roomId = InputUtils.PromptForInt("Room Id", "Enter the Room ID (or type 'exit')");
            if (roomId == -1) return;

            //verify room exists and is not deleted
            RoomDTO roomDTO = roomService.GetRoomById(roomId).Result;
            if (roomDTO == null || roomDTO.IsDeleted)
            {
                Error("The room with the given ID does not exist.");
                return;
            }

            int groupId = InputUtils.PromptForInt("Group Id", "Enter the Group ID (or type 'exit')");
            if (groupId == -1) return;

            //verify group exists and is not deleted
            GroupDTO groupDTO = groupService.GetGroupById(groupId).Result;
            if (groupDTO == null || groupDTO.IsDeleted)
            {
                Error("The group with the given ID does not exist.");
                return;
            }

            AccessDTO accessDTO = new AccessDTO
            {
                RoomId = roomId,
                GroupId = groupId
            };

            if (accessService.RevokeAccessAsync(accessDTO).Result)
                Success("Successfully revoked access.");
            else
                Error("An error occurred while revoking access.");
        }

        public string GetDescription() => $"{CommandName} : Revoke access from a group for a room.";
    }
}