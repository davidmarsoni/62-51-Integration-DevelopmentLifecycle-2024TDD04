using DTO;
using MVC.Services;
using ConsoleApp.utils;
using ConsoleApp.commands.interfaces;

namespace ConsoleApp.commands.Access
{
    public class Access : BaseCommand
    {
        private readonly AccessService accessService;

        public Access(HttpClient httpClient, string baseURL, bool debug)
            : base("access : Manage access to rooms for groups.", new Dictionary<string, ISubCommand>
            {
                { AccessGrant.CommandName, new AccessGrant(new AccessService(httpClient, baseURL, debug)) },
                { AccessRevoke.CommandName, new AccessRevoke(new AccessService(httpClient, baseURL, debug)) },
                { AccessList.CommandName, new AccessList(new AccessService(httpClient, baseURL, debug)) }
            })
        {
            accessService = new AccessService(httpClient, baseURL, debug);
        }
    }
}
