using ConsoleApp.console;
using ConsoleApp.utils;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // consts
            const string BASE_URL = "https://localhost:7284/api";
            const bool DEBUG = true;

            // init
            HttpClient httpClient = new HttpClient();
            ConsoleManager consoleManager = new ConsoleManager(httpClient, BASE_URL, DEBUG);
            consoleManager.Launch();
        }
    }
}
