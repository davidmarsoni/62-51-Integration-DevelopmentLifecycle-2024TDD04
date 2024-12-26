using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;
using ConsoleApp.commands.interfaces;
using ConsoleApp.commands.Room;

namespace ConsoleApp.commands.Room
{
    public class Room : BaseCommand
    {
        public static string CommandName => "room";
        public Room(HttpClient httpClient, string baseURL, bool debug)
            : base($"{CommandName} : Manage rooms.", new Dictionary<string, ISubCommand>
            {
                { RoomAdd.CommandName, new RoomAdd(new RoomService(httpClient, baseURL, debug)) },
                { RoomDelete.CommandName, new RoomDelete(new RoomService(httpClient, baseURL, debug)) },
                { RoomList.CommandName, new RoomList(new RoomService(httpClient, baseURL, debug)) },
                { RoomEdit.CommandName, new RoomEdit(new RoomService(httpClient, baseURL, debug)) }
            })
        {
        }
    }
}
