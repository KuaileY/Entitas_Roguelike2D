using Entitas;
using UnityEngine;
class GameController:MonoBehaviour
{
    Systems _systems;
    void Start()
    {
        var pools = Pools.sharedInstance;
        pools.SetAllPools();

        _systems = CreateSystems(pools);
        _systems.Initialize();
    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

    void OnDestory()
    {
        _systems.TearDown();
    }
    Systems CreateSystems(Pools pools)
    {
        return new Feature("Systems")
            //input
            .Add(pools.input.CreateSystem(new InputSystem()))
            .Add(pools.input.CreateSystem(new ProcessInputSystem()))
            .Add(pools.input.CreateSystem(new LevelSystem()))
            .Add(pools.input.CreateSystem(new TurnSystem()))
            .Add(pools.input.CreateSystem(new AISystem()))
            .Add(pools.input.CreateSystem(new GameOverSystem()))
            //create
            .Add(pools.core.CreateSystem(new CreateGameBoardCacheSystem()))
            .Add(pools.core.CreateSystem(new GameBoardSystem()))
            //behaviour
            .Add(pools.core.CreateSystem(new MoveSystem()))
            .Add(pools.core.CreateSystem(new AstarSystem()))
            .Add(pools.core.CreateSystem(new AttackSystem()))
            .Add(pools.core.CreateSystem(new GetHitSystem()))
            .Add(pools.core.CreateSystem(new AnimationSystem()))
            //render
            .Add(pools.CreateSystem(new UISystem()))
            .Add(pools.core.CreateSystem(new AddViewSystem()))
            .Add(pools.board.CreateSystem(new AddViewSystem()))
            .Add(pools.core.CreateSystem(new RemoveViewSystem()))
            //property
            .Add(pools.core.CreateSystem(new HpSystem()))
            //audio
            .Add(pools.input.CreateSystem(new AudioSystem()))
            //destory
            .Add(pools.core.CreateSystem(new DestorySystem()))
            ;
    }
}

