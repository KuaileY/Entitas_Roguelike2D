using System.Collections.Generic;
using Entitas;
public sealed class GetHitSystem : IReactiveSystem
{
    public TriggerOnEvent trigger { get { return CoreMatcher.GetHit.OnEntityAdded(); } }
    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("GetHitSystem execute.");
        foreach (var e in entities)
        {
            if (e.isMovable)
            {
                e.AddAnimation(e.view.gameObject.tag + Res.animations.Hit);
            }
            e.IsGetHit(false);
        }
    }

}
