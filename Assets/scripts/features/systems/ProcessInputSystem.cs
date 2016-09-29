using System.Collections.Generic;
using Entitas;
public sealed class ProcessInputSystem : IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger{ get { return InputMatcher.Input.OnEntityAdded(); } }
    Pool _pool;

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        
        if (_pool.isProcessing == true)
                return;
        TestLoadConfig.log.Trace("=======================");
        TestLoadConfig.log.Trace("InputSystem Execute.");
        //处理输入，暂停中
        _pool.isProcessing = true;
        //激活当前回合（触发TrunSystem)
        _pool.isActiveTurn = true;
        //_pool.isActiveAI = true;
    }


}

