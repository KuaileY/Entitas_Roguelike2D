using Entitas;
public sealed class UISystem : IInitializeSystem
{
    public void Initialize()
    {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();

        var contextSingle = Singleton<ContextManager>.Instance;
        contextSingle.Push(new GameStartContext());
    }
}

