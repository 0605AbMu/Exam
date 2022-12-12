using EventManagementSystem.Models;

namespace EventManagementSystem.UI;

public class UserView : View
{
    public override void Home(string message = null)
    {
        Console.Clear();
        if (message != null)
            Console.WriteLine(message);
        int index = GetUserMenuIndex("Eventlar ro'yxati", "Joy belgilash", "Men haqimda", "Chiqish");
        switch (index)
        {
            case 0:
                PrintListOfEvents();
                break;
            default:
                Home("To Do");
                break;
        }
    }

    private void PrintListOfEvents()
    {
        if (user.CompanyId == null)
        {
            Home("Sizda company ma'lumotlari bo'yicha muammo bor. Keyinroq urinib ko'ring");
            return;
        }

        var applications = _tool.ExecuteQueryAndReturnAsAsync<Application>(
            new($"SELECT * FROM [Application] WHERE id = {user.CompanyId}"),
            typeof(Application)).Result.Where(x =>
            x.Status == ApplicationStatus.Processing || x.Status == ApplicationStatus.Accepted);
        if (applications.Count() == 0)
        {
            Home("Sizda active eventlar yo'q");
            return;
        }

        Console.WriteLine("Ma'lumot olish uchun tanlang: ");
        int index = GetUserMenuIndex(applications.Select(x => $"{x.StartAt}-{x.EndAt}").ToArray());
        PrintEvent(application: applications.ElementAtOrDefault(index));
    }

    void PrintEvent(Application application)
    {
        if (application == null)
        {
            Home("Noma'lum xatolik!");
            return;
        }

        var room = _tool
            .ExecuteQueryAndReturnAsAsync<Room>(new($"SELECT * FROM [Room] WHERE id = {application.RoomId}"), typeof(Room)).Result
            .FirstOrDefault();
        Console.WriteLine($@"Boshlanish vaqti: {application.StartAt};
Tugash vaqti: {application.EndAt};
Xona nomi: {room?.Name ?? "Noma'lum"}
Xona Sig'imi: {room?.capacity ?? 0} 
");
        Console.WriteLine("Chiqish uchun biror tugmani bosing....");
        Console.Read();
        Home();
        return;
    }
}