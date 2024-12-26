using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using Microsoft.IdentityModel.Tokens;
using MVC.Services;
using MVC.Services.Interfaces;

namespace ConsoleApp.commands.History
{
    public class History : BaseCommand
    {   
        public static string CommandName => "history";
        public History(HttpClient httpClient, string baseURL, bool debug)
            : base($"{CommandName} : Manages the history.", new Dictionary<string, ISubCommand>
            {
                { HistoryList.CommandName, new HistoryList(new AccessLogService(httpClient, baseURL, debug)) }
            })
        {
        }
    }
}
