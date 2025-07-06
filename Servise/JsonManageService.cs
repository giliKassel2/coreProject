using System.Text.Json;

using namespace myProject.Services;
public class JsonManageService
{
    public static List<T> LoadFromJson<T>(string filePath)
    {
        if (!File.Exists(filePath))
            return new List<T>();

        using (var json = File.OpenText(filePath))
        {
            return JsonSerializer.Deserialize<List<T>>(json.ReadToEnd(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<T>();
        }
    }

    public static void SaveToJson<T>(string filePath, List<T> entities)
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