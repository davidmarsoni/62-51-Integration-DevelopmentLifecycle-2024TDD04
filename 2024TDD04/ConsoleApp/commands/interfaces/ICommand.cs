namespace ConsoleApp.commands.interfaces
{
    public interface ICommand
    {
        public void Execute(string[] arguments);
        static string? CommandName { get; }
        public string GetDescription();
        public string GetSubCommands();
    }
}
