using ConsoleApp.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands
{
    public class Room : ICommand
    {
        private RoomService roomService;

        public Room(HttpClient httpClient, string baseURL, bool debug)
        {
            roomService = new RoomService(httpClient, baseURL);
        }

        public void Execute(string[] arguments)
        {
            // Replace NotImplementedException with actual implementation
            if (arguments.Length == 0)
            {
                Console.WriteLine("Room : " + Colors.Colorize("No arguments provided", Colors.Red));
                return;
            }
            switch (arguments[0])
            {
                case "add":
                    AddRoom();
                    break;
                case "delete":
                    DeleteRoom();
                    break;
                case "list":
                    ListRooms();
                    break;
                case "edit":
                    EditRoom();
                    break;
                default:
                    Console.WriteLine("Room : " + Colors.Colorize("Command not found", Colors.Red));
                    break;
            }
        }

        public string GetDescription()
        {
            return "room - Manage rooms.";
        }

        public string GetSubCommands()
        {
            // Replace NotImplementedException with actual implementation
            return "add, delete, list, edit";
        }

        // methods
        private void ListRooms()
        {
            IEnumerable<RoomDTO>? roomDTOs = roomService.GetAllRooms().Result;
            EntityCommandUtils.ListEntities(roomDTOs, "Room", room =>
            {
                string abreviation = string.IsNullOrEmpty(room.RoomAbreviation) ? "" : $" ({room.RoomAbreviation})";
                return $"{room.Id} - {room.Name}{abreviation}";
            });
        }

        private void AddRoom()
        {
            Console.WriteLine("Beginning the \"Add Room\" process...");
            Console.WriteLine("Enter a " + Colors.Colorize("Name", Colors.Yellow) + " to create a room.");
            string nameInput = ConsoleManager.WaitInput(ValidateRoomNameIsUnique, "Enter the room name (or type 'exit')").Trim();
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting room creation."))
                return;
            Console.WriteLine("Enter an optional " + Colors.Colorize("Abreviation", Colors.Yellow) + " for the room.");
            string abreviationInput = ConsoleManager.WaitInput(ValidateRoomAbreviationIsUnique, "(Press enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(abreviationInput, "Exiting room creation."))
                return;
            RoomDTO roomDTO = new RoomDTO
            {
                Name = nameInput,
                RoomAbreviation = string.IsNullOrEmpty(abreviationInput) ? null : abreviationInput
            };
            if (roomService.CreateRoom(roomDTO).Result != null)
                Console.WriteLine(Colors.Colorize("Successfully added the room.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when adding the room to the DB...", Colors.Red));
        }

        private void EditRoom()
        {
            Console.WriteLine("Beginning the \"Edit Room\" process...");
            Console.WriteLine("Enter the " + Colors.Colorize("Room Id", Colors.Yellow) + " to edit a room.");
            string roomIdInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To edit a room, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')").ToLower();
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting room editing."))
                return;
            int roomId;
            try
            {
                roomId = int.Parse(roomIdInput);
            }
            catch
            {
                Console.WriteLine(Colors.Colorize("Invalid Room Id. Exiting...", Colors.Red));
                return;
            }
            RoomDTO? roomDTO = roomService.GetRoomById(roomId).Result;
            if (roomDTO == null)
            {
                Console.WriteLine(Colors.Colorize("Room not found. Exiting...", Colors.Red));
                return;
            }
            Console.WriteLine("Enter a new " + Colors.Colorize("Name", Colors.Yellow) + " for the room. (Press Enter to skip)");
            string nameInput = ConsoleManager.WaitInput(ValidateRoomNameIsUniqueForEdit, "(Press Enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(nameInput, "Exiting room editing."))
                return;

            Console.WriteLine("Enter a new " + Colors.Colorize("Abreviation", Colors.Yellow) + " for the room. (Press Enter to skip)");
            string abreviationInput = ConsoleManager.WaitInput(ValidateRoomAbreviationIsUnique, "(Press Enter to skip)").Trim();
            if (ConsoleUtils.ExitOnInputExit(abreviationInput, "Exiting room editing."))
                return;

            if (string.IsNullOrEmpty(nameInput) && string.IsNullOrEmpty(abreviationInput))
            {
                Console.WriteLine("No changes made. Exiting room editing.");
                return;
            }

            if (!string.IsNullOrEmpty(nameInput))
                roomDTO.Name = nameInput;

            if (!string.IsNullOrEmpty(abreviationInput))
                roomDTO.RoomAbreviation = abreviationInput;

            if (roomService.UpdateRoom(roomDTO).Result)
                Console.WriteLine(Colors.Colorize("Successfully edited the room.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize("An error occurred when editing the room in the DB...", Colors.Red));
        }

        private void DeleteRoom()
        {
            Console.WriteLine("Beginning the \"Delete Room\" process...");
            Console.WriteLine("Enter the " + Colors.Colorize("Room Id", Colors.Yellow) + " to delete a room.");
            string roomIdInput = ConsoleManager.WaitInput(
                EntityCommandUtils.ValidationIdIsInt,
                "To delete a room, input the " + Colors.Colorize("ID", Colors.Yellow) + ". (or type 'exit')").ToLower();
            if (ConsoleUtils.ExitOnInputExit(roomIdInput, "Exiting room deletion."))
                return;
            int roomId;
            try
            {
                roomId = int.Parse(roomIdInput);
            }
            catch
            {
                Console.WriteLine(Colors.Colorize("An error occurred when parsing the roomId. Exiting...", Colors.Red));
                return;
            }
            EntityCommandUtils.ConfirmAndDeleteEntity(roomId, roomService.DeleteRoom, "Room");
        }

        // validation methods
        private bool ValidateRoomNameIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            if (input.ToLower() == "exit")
                return true;
            if (roomService.RoomNameExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Room name already exists. Please choose a different name.", Colors.Red));
                return false;
            }
            return true;
        }

        private bool ValidateRoomNameIsUniqueForEdit(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                return true;
            if (roomService.RoomNameExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Room name already exists. Please choose a different name.", Colors.Red));
                return false;
            }
            return true;
        }

        private bool ValidateRoomAbreviationIsUnique(string input)
        {
            if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                return true;
            if (roomService.RoomAbreviationExists(input).Result)
            {
                Console.WriteLine(Colors.Colorize("Room abreviation already exists. Please choose a different abreviation.", Colors.Red));
                return false;
            }
            return true;
        }
    }
}
