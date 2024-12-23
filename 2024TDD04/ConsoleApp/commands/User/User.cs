using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.User
{
    public class User : BaseCommand
    {
        private readonly UserService userService;

        public User(HttpClient httpClient, string baseURL, bool debug)
            : base("user : Manage users.", new Dictionary<string, ISubCommand>
            {
                { UserAdd.CommandName, new UserAdd(new UserService(httpClient, baseURL, debug)) },
                { UserDelete.CommandName, new UserDelete(new UserService(httpClient, baseURL, debug)) },
                { UserList.CommandName, new UserList(new UserService(httpClient, baseURL, debug)) },
                { UserEdit.CommandName, new UserEdit(new UserService(httpClient, baseURL, debug)) }
            })
        {
            userService = new UserService(httpClient, baseURL, debug);
        }
    }
}
