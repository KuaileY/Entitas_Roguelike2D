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
                sound(e); 
            }
            e.IsGetHit(false);
        }
    }

    void sound(Entity entity)
    {
        if (entity.asset.name == Res.enemy1)
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_enemy1);
        if (entity.asset.name == Res.enemy2)
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_enemy2);
    }

}
