namespace EventManagementSystem.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int capacity { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}