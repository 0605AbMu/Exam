using System.Globalization;
using EventManagementSystem.Models;
using StorageBroker;

namespace EventManagementSystem.UI;

public abstract class View
{
    public string userInputAsString = "";
    protected DataBaseTool _tool;
    public User user { get; set; }

    public void SetDataBaseTool(DataBaseTool tool) => this._tool = tool;
    public abstract void Home(string message = null);

    public int GetUserMenuIndex(params string[] menuItems)
    {
        Console.WriteLine(String.Join("\n", menuItems.Select((x, index) => $"{index + 1}.{x}")));
        Console.Write(">>> ");
        int index = 0;
        int.TryParse(Console.ReadLine(), out index);
        return index - 1;
    }
}