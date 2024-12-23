using ConsoleApp.console;

namespace ConsoleApp.utils
{
    public static class EntityCommandUtils
    {
        public static void ListEntities<T>(IEnumerable<T>? entities, string entityName, Func<T, string> displayFunction, IEnumerable<string>? initialLines = null)
        {
            if (initialLines != null)
            {
                foreach (var line in initialLines)
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine($"Listing {entityName.ToLower()}s...");
            if (entities == null)
            {
                Console.WriteLine($"{entityName} : " + Colors.Colorize($"Error fetching {entityName.ToLower()}s", Colors.Red));
                return;
            }
            if (!entities.Any())
            {
                Console.WriteLine($"{entityName} : " + Colors.Colorize($"No {entityName.ToLower()}s found", Colors.Orange));
                return;
            }
            foreach (var entity in entities)
            {
                Console.WriteLine(displayFunction(entity));
            }
        }

        public static void ConfirmAndDeleteEntity(int entityId, Func<int, Task<bool>> deleteFunction, string entityName)
        {
            Console.WriteLine($"Beginning the \"Delete {entityName}\" process...");
            Console.WriteLine(Colors.Colorize($"Are you sure you want to delete the {entityName.ToLower()}? (type 'y' to confirm)", Colors.Red));
            Console.Write("> ");
            string confirm = Console.ReadLine().ToLower();
            if (confirm != "y")
            {
                Console.WriteLine($"Exiting {entityName.ToLower()} deletion.");
                return;
            }
            Console.WriteLine(Colors.Colorize($"Deleting the {entityName.ToLower()}...", Colors.Red));
            bool entityDeleted = deleteFunction(entityId).Result;
            if (entityDeleted)
                Console.WriteLine(Colors.Colorize($"Successfully deleted the {entityName.ToLower()}.", Colors.Green));
            else
                Console.WriteLine(Colors.Colorize($"An error occurred when deleting the {entityName.ToLower()} from the DB...", Colors.Red));
        }
    }
}