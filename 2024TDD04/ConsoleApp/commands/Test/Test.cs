using MVC.Services;
using DTO;
using MVC.Services.Interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using ConsoleApp.commands.interfaces;

namespace ConsoleApp.commands.Test
{
    public class Test : BaseCommand
    {
        public Test(HttpClient httpClient, string baseURL, bool debug)
            : base("test : Commands for testing purposes.", new Dictionary<string, ISubCommand>
            {
                { TestAccess.CommandName, new TestAccess(new TestService(httpClient, baseURL, debug)) }
            })
        {
        }
    }
}
