using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;

public sealed class MoveSystem : IReactiveSystem
{
    public TriggerOnEvent trigger { get{return CoreMatcher.Move.OnEntityAdded();} }
    Queue<Entity> _moveEntities = new Queue<Entity>();
    bool _processing;
    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("MoveSystem execute.");
        foreach (var entity in entities)
        {
            _moveEntities.Enqueue(entity);
        }
        if (_processing)
            return;
        Move(_moveEntities.Dequeue());
    }
    void Move(Entity entity)
    {
        _processing = true;
        sound(entity); 
        entity.view.gameObject.transform.DOMove(new Vector2(entity.position.x, entity.position.y), 0.2f)
            .OnComplete(() =>
            {
                TestLoadConfig.log.Trace(string.Format("{0} MoveComponent remove.", entity.view.gameObject.name));
                entity.IsMove(false);
                if (_moveEntities.Count > 0)
                    Move(_moveEntities.Dequeue());
                else
                    _processing = false;
            });

    }

    void sound(Entity entity)
    {
        if (entity.view.gameObject.tag != Res.player)
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_footstep2);
    }


}

