using MarketFlowCLI.Model;

namespace MarketFlowCLI.Repository;


public class Repository<T> where T : IEntity
{

    private readonly Dictionary<Guid, T> _items = new();

    public int Count => _items.Count;


    public void Add(T item)
    {
        _items[item.Id] = item;
    }


    public T? FindById(Guid id) => _items.TryGetValue(id, out var item) ? item : default;

    public IReadOnlyCollection<T> FindAll() => _items.Values.ToList().AsReadOnly();
}
