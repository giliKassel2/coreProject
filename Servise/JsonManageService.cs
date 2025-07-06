using System.Text.Json;

 namespace myProject.Services;
public class JsonManageService<T>
{
    public static List<T> LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
            return new List<T>();

        using (var json = File.OpenText(filePath))
        {
            return JsonSerializer.Deserialize<List<T>>(json.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<T>();
        }
    }

    public static void SaveToJson(string filePath, List<T> entities)
    {
        try
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(entities, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to JSON: {ex.Message}");   
        }
    }
}