namespace myProject.Models
;
    public class Teacher
    {
        public int Id { get; set; }
        public string HashPassword{ get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public List<string> Clases { get; set; } = new List<string>();
    }
    