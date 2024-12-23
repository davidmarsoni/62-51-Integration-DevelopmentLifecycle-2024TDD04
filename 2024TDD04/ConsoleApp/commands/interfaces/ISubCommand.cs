namespace ConsoleApp.commands.interfaces
{
    public interface ISubCommand
    {
        void Execute(string[] arguments);
        string GetDescription();
        static string CommandName { get; }
    }
}
