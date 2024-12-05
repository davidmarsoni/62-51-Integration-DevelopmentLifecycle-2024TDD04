using ConsoleApp.commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.commands
{
    public class Access : ICommand
    {
        public void Execute(string[] arguments)
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            return "access - Access a room.";
        }

        public string GetSubCommands()
        {
            return "Available subcommands: create, delete, list, add, remove";
        }
    }
}
