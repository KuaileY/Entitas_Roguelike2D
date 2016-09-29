using System.Collections.Generic;
using Entitas;
public sealed class DestorySystem : IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Destory.OnEntityAdded(); } }
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            _pool.DestroyEntity(e);
        }
    }

}

