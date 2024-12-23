namespace ConsoleApp.commands.interfaces
{
    public interface ICommand
    {
        public void Execute(string[] arguments);
        public string GetDescription();
        public string GetSubCommands();
    }
}
