using ConsoleApp.commands.interfaces;
using ConsoleApp.console;
using ConsoleApp.utils;
using DTO;
using MVC.Services;

namespace ConsoleApp.commands.Group
{
    public class Group : BaseCommand
    {
        public static string CommandName => "group";

        public Group(HttpClient httpClient, string baseURL, bool debug)
            : base($"{CommandName} : Manage groups.", new Dictionary<string, ISubCommand>
            {
                { GroupAdd.CommandName, new GroupAdd(new GroupService(httpClient, baseURL, debug)) },
                { GroupAddUser.CommandName, new GroupAddUser(new GroupService(httpClient, baseURL, debug), new UserGroupService(httpClient, baseURL, debug), new UserService(httpClient, baseURL, debug)) },
                { GroupDelete.CommandName, new GroupDelete(new GroupService(httpClient, baseURL, debug)) },
                { GroupEdit.CommandName, new GroupEdit(new GroupService(httpClient, baseURL, debug)) },
                { GroupList.CommandName, new GroupList(new GroupService(httpClient, baseURL, debug)) },
                { GroupListUsers.CommandName, new GroupListUsers(new GroupService(httpClient, baseURL, debug), new UserGroupService(httpClient, baseURL, debug)) },
                { GroupRemoveUser.CommandName, new GroupRemoveUser(new GroupService(httpClient, baseURL, debug), new UserGroupService(httpClient, baseURL, debug), new UserService(httpClient, baseURL, debug)) }
            }){}
    }
}
