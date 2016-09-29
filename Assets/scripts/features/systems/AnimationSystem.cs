using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AnimationSystem : IReactiveSystem
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Animation.OnEntityAdded(); } }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            TestLoadConfig.log.Trace(string.Format("{0} AnimationSystem Execute.", e.animation.trigger));
            var trigger = e.animation.trigger;
            var gameObject = e.view.gameObject;

            var animator = gameObject.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger(trigger);
            e.RemoveAnimation();

        }
    }

}

