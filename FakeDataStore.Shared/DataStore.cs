namespace FakeDataStore.Shared;

public static class DataStore<T> where T : class
{
    private static List<T> _list = [];

    public static List<T> Get()
    {
        return _list;
    }

    public static void Update(List<T> newList)
    {
        _list.Clear();
        foreach (var item in newList)
        {
            _list.Add(item);
        }
    }
}
