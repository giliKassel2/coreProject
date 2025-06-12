namespace myProject.Models;
  public class Student 
  {
   // internal int id;
    public string HashPassword { get; set; }
    public int Id {get;set;}
    public string Name {get;set;}
    
    public List<Presence> presence {get;set;} 
    // = List<Presence>

  }