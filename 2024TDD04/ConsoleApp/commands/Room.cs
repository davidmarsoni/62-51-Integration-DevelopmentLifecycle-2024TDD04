using ConsoleApp.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.commands
{
    public class Room : ICommand
    {
        public void Execute(string[] arguments)
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            return "room - Manages rooms.";
        }

        public string GetSubCommands()
        {
            throw new NotImplementedException();
        }
    }
}
