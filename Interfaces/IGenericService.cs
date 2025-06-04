namespace myProject.Interfaces;
public interface IGenericService<T>
{
   // public int Id { get; set; }
    List<T> Get();
    T Create(T entity);
    T ?Update( T entity ,Func<T, bool> predicate);
    bool Delete(Func<T, bool> predicate);
}