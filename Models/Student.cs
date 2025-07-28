using myProject.Models;
  namespace myProject.Models;

 public class Student 
 {
   // internal int id;

  public int Id { get; set; }
  public string Name { get; set; }
  public List<Presence> presence { get; set; }
  public string Clas { get; set; }

}