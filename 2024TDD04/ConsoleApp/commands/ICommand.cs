using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.commands
{
    public interface ICommand
    {
        public void Execute(String[] arguments);
        public string GetDescription();
        public string GetSubCommands();
    }
}
