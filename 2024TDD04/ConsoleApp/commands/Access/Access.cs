using MVC.Services;
using ConsoleApp.commands.interfaces;

namespace ConsoleApp.commands.Access
{
    public class Access : BaseCommand
    {
        public static string CommandName => "access";
        public Access(HttpClient httpClient, string baseURL, bool debug)
            : base($"{CommandName} : Manage access to rooms for groups.", new Dictionary<string, ISubCommand>
            {
                { AccessGrant.CommandName, new AccessGrant(new AccessService(httpClient, baseURL, debug), new GroupService(httpClient, baseURL, debug), new RoomService(httpClient, baseURL, debug)) },
                { AccessRevoke.CommandName, new AccessRevoke(new AccessService(httpClient, baseURL, debug), new GroupService(httpClient, baseURL, debug), new RoomService(httpClient, baseURL, debug)) },
                { AccessList.CommandName, new AccessList(new AccessService(httpClient, baseURL, debug),new GroupService(httpClient, baseURL, debug), new UserService(httpClient, baseURL, debug)) }
            })
        {
        }
    }
}
