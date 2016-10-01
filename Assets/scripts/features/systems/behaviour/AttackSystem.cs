using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public sealed class AttackSystem : IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Attack.OnEntityAdded(); }}
    Queue<Entity> _attackEntities = new Queue<Entity>();
    bool _processing;
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("attackSystem execute.");
        foreach (var entity in entities)
        {
            _attackEntities.Enqueue(entity);
        }
        if (_processing)
            return;
        Attack(_attackEntities.Dequeue());
    }
    void Attack(Entity entity)
    {
        _processing = true;
        sound(entity); 
        entity.AddAnimation(entity.view.gameObject.tag + Res.animations.Attack);
        var x = entity.view.gameObject.transform.position.x + entity.attack.dir.x;
        var y = entity.view.gameObject.transform.position.y + entity.attack.dir.y;
        _pool.gameBoardCache.grid[(int)x, (int)y].IsGetHit(true);
        entity.delay(0.1f, () =>
        {
            TestLoadConfig.log.Trace(string.Format("{0} AttackComponent remove.", entity.view.gameObject.name));
            entity.RemoveAttack();
            if (_attackEntities.Count > 0)
                Attack(_attackEntities.Dequeue());
            else
                _processing = false;
        });

    }

    void sound(Entity entity)
    {
        if (entity.view.gameObject.tag == Res.player)
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_chop1);
        else
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_chop2);
    }

}

