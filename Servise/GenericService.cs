using System.Text.Json;
using myProject.Controllers;
using myProject.Interfaces;


namespace myProject.Services;

public class GenericService<T> : IGenericService<T>
{
    private readonly string _filePath;
    private List<T> _entities;

    public GenericService(List<T> entities)
    {
        this._entities = entities ?? new List<T>();
        // _filePath = filePath;

        // // Load entities from JSON file
        // if (File.Exists(_filePath))
        // {
        //     System.Console.WriteLine("tbh ");
        //     using (var json = File.OpenText(_filePath))
        //     {
        //         _entities = JsonSerializer.Deserialize<List<T>>(json.ReadToEnd(),
        //             new JsonSerializerOptions
        //             {
        //                 PropertyNameCaseInsensitive = true
        //             }) ?? new List<T>();
        //     }
        //     System.Console.WriteLine(_entities);
        // }
        // else
        // {
        //     _entities = new List<T>();
        // }
    }

    public GenericService()
    {
    }

    public List<T> Get()
    {
        return _entities;
    }

    public T? Get(Func<T, bool> predicate)
    {
        //  Console.WriteLine("************************");
        // foreach (var entity in _entities)
        // {
        //     Console.WriteLine(JsonSerializer.Serialize(entity));
        // }
        return _entities.FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _entities.Add(entity);
        SaveToJson();
        return entity;
    }

    public T? Update(T entity, Func<T, bool> predicate)
    {
        var existingEntity = _entities.FirstOrDefault(predicate);
        if (existingEntity != null)
        {
            var index = _entities.IndexOf(existingEntity);
            _entities[index] = entity;
            SaveToJson();
            return existingEntity;
        }
        return default(T);
    }

    public bool Delete(Func<T, bool> predicate)
    {
        var entityToRemove = _entities.FirstOrDefault(predicate);
        if (entityToRemove != null)
        {
            _entities.Remove(entityToRemove);
            SaveToJson();
            return true;
        }
        return false;
    }

    private void SaveToJson()
    {
        try
        {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_entities, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to JSON: {ex.Message}");
        }
    }



}
