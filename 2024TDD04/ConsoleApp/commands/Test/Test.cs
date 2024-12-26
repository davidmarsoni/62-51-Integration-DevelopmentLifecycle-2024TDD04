using MVC.Services;
using ConsoleApp.commands.interfaces;

namespace ConsoleApp.commands.Test
{
    public class Test : BaseCommand
    {
        public static string CommandName => "test";
        public Test(HttpClient httpClient, string baseURL, bool debug)
            : base($"{CommandName} : Commands for testing purposes.", new Dictionary<string, ISubCommand>
            {
                { TestAccess.CommandName, new TestAccess(new TestService(httpClient, baseURL, debug), new AccessLogService(httpClient, baseURL, debug)) }
            })
        {
        }
    }
}
