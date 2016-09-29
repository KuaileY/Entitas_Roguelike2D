using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AISystem : IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.ActiveAI.OnEntityAdded(); } }

    List<Entity> moveList = new List<Entity>();
    Dictionary<Entity, Vector2> attackList = new Dictionary<Entity, Vector2>();

    Group enemies;
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        enemies = pools.core.GetGroup(CoreMatcher.ContainAI);

        _pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Attack,CoreMatcher.ContainAI)).OnEntityRemoved += OnRemoveAttackList;
        _pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Move, CoreMatcher.ContainAI)).OnEntityRemoved += OnRemoveMoveList;
    }

    #region 移除行动元素
    private void OnRemoveAttackList(Group @group, Entity entity, int index, IComponent component)
    {
        attackList.Remove(entity);
        TestLoadConfig.log.Trace(string.Format("Remove attackList:{0}", attackList.Count));
        checkAttack();
    }
    private void OnRemoveMoveList(Group @group, Entity entity, int index, IComponent component)
    {
        moveList.Remove(entity);
        TestLoadConfig.log.Trace(string.Format("Remove moveList:{0}", moveList.Count));
        checkMove();
    }
    //检测是否移动完成
    void checkMove()
    {
        if (_pools.input.isActiveAI && moveList.Count == 0)
        {
            TestLoadConfig.log.Trace("AIEnd.");
            _pools.input.isProcessing = false;
            _pools.input.isActiveAI = false;
        }
    }
    //检测是否攻击完成
    void checkAttack()
    {
        if (_pools.input.isActiveAI && attackList.Count == 0)
        {
            move();
        }
    }

    #endregion

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("AISystem Execute");
        RoleBehaviour();
    }

    //角色行为
    void RoleBehaviour()
    {
        foreach (var e in enemies.GetEntities())
        {
            Vector2 dir;
            var boardCache = _pools.core.gameBoardCacheEntity.gameBoardCache;
            if (e.Around(boardCache, out dir))
                attackList.Add(e, dir);
            else
                moveList.Add(e);
        }
        attack();
    }

    //移动
    void move()
    {
        TestLoadConfig.log.Trace(string.Format("moveList Count:{0}.", moveList.Count));
        if (moveList.Count == 0)
            checkMove();
        foreach (var e in moveList)
        {
            //建立玩家角色下一步坐标
            e.IsAstar(true);
        }
    }

    //攻击
    void attack()
    {
        TestLoadConfig.log.Trace(string.Format("attackList Count:{0}.", attackList.Count));
        if (attackList.Count == 0)
            move();
        foreach (KeyValuePair<Entity, Vector2> kv in attackList)
        {
            kv.Key.AddAttack(kv.Value);
            if (kv.Key.asset.name == Res.enemy1)
                updateHp(Res.enemy1Dmg);
            if (kv.Key.asset.name == Res.enemy2)
                updateHp(Res.enemy2Dmg);
        }
    }

    //更新血量
    void updateHp(int num)
    {
        var player = _pools.core.controlableEntity;
        player.ReplaceHp(player.hp.curValue + num);
    }

}


