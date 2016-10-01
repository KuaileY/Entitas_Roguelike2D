using System.Collections.Generic;
using Entitas;
using UnityEngine.UI;

public sealed class GameOverSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.GameOver.OnEntityAdded(); } }
    Pools _pools;
    Group _interactriveEntities;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _interactriveEntities = pools.core.GetGroup(CoreMatcher.Interactive);
        pools.input.GetGroup(InputMatcher.ActiveAI).OnEntityRemoved += OnClearLevel;
    }
    //清理关卡
    private void OnClearLevel(Group @group, Entity entity, int index, IComponent component)
    {
        if (_pools.input.isGameOver)
        {
            foreach (var e in _interactriveEntities.GetEntities())
                e.IsDestory(true);
        }
    }

    public void Execute(List<Entity> entities)
    {
        _pools.input.isProcessing = true;
        _pools.input.CreateEntity().AddEfxSound(Res.audios.scavengers_die);
        _pools.input.DestroyEntity(_pools.input.musicEntity);
        Singleton<ContextManager>.Instance.Pop();
        Singleton<ContextManager>.Instance.Push(new GameOvertContext());

        Singleton<UIManager>.Instance.GetSingleUI(Res.GameOver).GetComponentInChildren<Text>().text =
            string.Format("You save {0} days!!!", _pools.input.level.value);
    }


}

