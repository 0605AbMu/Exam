using System.Data.SqlClient;
using EventManagementSystem.Models;
using StorageBroker;

namespace EventManagementSystem.UI;

public class Main
{
    private DataBaseTool tool;
    private Admin adminView = new Admin();
    private IList<User> SessionUsers = new List<User>();

    public Main(DataBaseTool tool)
    {
        this.tool = tool;
    }

    public void Login(string message = null)
    {
        Console.Clear();
        if (message != null)
            Console.WriteLine(message);
        Console.Write(@"username kiriting: ");
        var s = Console.ReadLine();
        User? user = (tool.ExecuteQueryAndReturnAsAsync<User>(
            new SqlCommand($"SELECT * FROM VIEW_GetALLUsers WHERE username = '{s}'"),
            typeof(User))).Result.FirstOrDefault();
        if (user == null)
        {
            Login("Bunday username ga ega foydalanuvchi topilmadi");
            return;
        }

        Console.Write("Parolingizni kiriting: ");
        s = Console.ReadLine();
        if (user.Password != s)
        {
            Login("username yoki password xato");
            return;
        }

        ForkToView(user);
    }

    void ForkToView(User user)
    {
        View view;
        if (user.Role == Role.Admin)
            view = new Admin();
        // Assign spesific views with condition to view variable
        else
            view = new UserView();
        view.user = user;
        view.SetDataBaseTool(tool);
        view.Home();
    }
}