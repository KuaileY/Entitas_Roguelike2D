
public static class Singleton<T> where T : class
{
    private static T _instance;

    public static void Create()
    {
        _instance = (T) System.Activator.CreateInstance(typeof (T), true);
    }

    public static T Instance
    {
        get { return _instance; }
    }

    public static void Deatory()
    {
        _instance = null;
    }
}
