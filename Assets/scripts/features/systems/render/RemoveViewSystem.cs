using System.Collections.Generic;
using Entitas;

public sealed class RemoveViewSystem:ISetPool,IReactiveSystem,IEnsureComponents
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Asset.OnEntityRemoved(); } }
    public IMatcher ensureComponents { get {return CoreMatcher.View; } }
    Pool pool;
    public void SetPool(Pool pool)
    {
        pool.GetGroup(CoreMatcher.View).OnEntityRemoved += (group, entity, index, component) =>
        {
            var viewComponent = component as ViewComponent;
            UnityEngine.Object.Destroy(viewComponent.gameObject);
        };
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            e.RemoveView();
        }
    }

}

