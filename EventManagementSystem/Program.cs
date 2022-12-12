using System;
using System.Data.SqlClient;
using EventManagementSystem.Models;
using StorageBroker;

class Program
{
    public static void Main(string[] args)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
        connectionStringBuilder.DataSource = "localhost, 60074";
        connectionStringBuilder.UserID = "sa";
        connectionStringBuilder.Password = "12345678";
        connectionStringBuilder.InitialCatalog = "EVENT_MANAGEMENT";
        DataBaseTool tool = new DataBaseTool(connectionStringBuilder);
        var mainController = new EventManagementSystem.UI.Main(tool);
        mainController.Login();
    }
}