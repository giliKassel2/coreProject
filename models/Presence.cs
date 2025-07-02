namespace myProject.Models;

public class Presence
{
    public DateTime Date { get; set; }
    public string Lesson { get; set; }
    public bool IsPresent { get; set; }
    public string TeacherId { get; set; }   
}