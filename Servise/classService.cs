using MyProject.models;
using myProject.Controllers;
using myProject.Interfaces;
using System.Text.Json;
namespace myProject.Servise;
public  class ClassService : IClasService
{
    private  List<Student>? clas;
    private static string file = "classData.json";
    private string filePath;
     public ClassService(IHostEnvironment environment)
    {
        filePath = Path.Combine(environment.ContentRootPath,"Data",file);
        
        using (var json = File.OpenText(filePath)){
            clas= JsonSerializer.Deserialize<List<Student>>(json.ReadToEnd(),
             new JsonSerializerOptions
             {
                PropertyNameCaseInsensitive = true
            });
        }
    }
    private void saveJson(){
        try{
           File.WriteAllText(filePath,JsonSerializer.Serialize(clas)); 
        }
        catch (Exception ex){
        Console.WriteLine($"Error saving to JSON: {ex.Message}");
        }
        
    }
    public  List<Student> Get(){  
        return clas;
    }

    public  Student Get(int id){
        Student currentStudent = clas.FirstOrDefault(s => s.Id == id);
        return currentStudent;
    }

    public  int Insert(Student s ){
        
        if( s == null  || string.IsNullOrWhiteSpace(s.Name)){
            return -1;
        }
        int maxId =clas.Any() ? clas.Max( s => s.Id) : 100;
        s.Id = maxId+1;
        // s.presence = new List<Presence>();
        // s.presence.Add(new Presence { Date = DateTime.Now, IsPresent = true });
        System.Console.WriteLine($"Before Add: {clas.Count}");
        this.clas.Add(s);
        saveJson();
        System.Console.WriteLine($"After Add: {clas.Count}");
        return s.Id;
    }

    public  bool UpDate(int id , Student s){
        if (isEmpty(s))
            return false;
        
        Student currentStudent = clas.FirstOrDefault( s => s.Id == id);
        if(currentStudent == null)
            return false;
        
        currentStudent.Name = s.Name;
        // currentStudent.presence = s.presence;
        saveJson();
        return true;
    }

    public  bool Delete( int id){
        Student currentStudent = Get(id);
        System.Console.WriteLine(currentStudent);
        if(currentStudent ==null)
        return false;

        clas.Remove(currentStudent);
        saveJson();
        return true;
    }

      public  bool isEmpty(Student  s)
    {
        return s == null || string.IsNullOrWhiteSpace (s.Name);
    }
}