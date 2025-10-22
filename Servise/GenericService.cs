using System.Text.Json;
using myProject.Controllers;
using myProject.Interfaces;


namespace myProject.Services;

public class GenericService<T> : IGenericService<T>
{
    private readonly string filePath;
    private List<T> _entities;

    public GenericService(List<T> entities,string filePath)
    {
        this._entities = entities ?? new List<T>();
        this.filePath = filePath;
        
    }


    public List<T> Get()
    {
        return _entities;
    }

    public T? Get(Func<T, bool> predicate)
    {
        System.Console.WriteLine("in get");
        return _entities.FirstOrDefault(predicate);
    } 

    public T Create(T entity)
    {
        _entities.Add(entity);
        JsonManageService<T>.SaveToJson(filePath , _entities);
        return entity;
    }

    public T? Update(T entity, Func<T, bool> predicate)
    {
        var existingEntity = _entities.FirstOrDefault(predicate);
       foreach (var prop in typeof(T).GetProperties())
        {
            var newValue = prop.GetValue(entity);
            if (newValue != null && !(newValue is string s && string.IsNullOrWhiteSpace(s)))
            {
                prop.SetValue(existingEntity, newValue);
            }
        }

        JsonManageService<T>.SaveToJson(filePath, _entities);
        return existingEntity;
    }

    public bool Delete(Func<T, bool> predicate)
    {
        var entityToRemove = _entities.FirstOrDefault(predicate);
        if (entityToRemove != null)
        {
            _entities.Remove(entityToRemove);
            JsonManageService<T>.SaveToJson(filePath , _entities);
            return true;
        }
        return false;
    }

    // private void SaveToJson()
    // {
    //     try
    //     {
    //         File.WriteAllText(_filePath, JsonSerializer.Serialize(_entities, new JsonSerializerOptions { WriteIndented = true }));
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error saving to JSON: {ex.Message}");
    //     }
    // }



}
