using System;
using System.Collections.Generic;
using System.ComponentModel;
using Entitas;
using UnityEngine;
using IComponent = Entitas.IComponent;
using Random = UnityEngine.Random;

public sealed class TurnSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.ActiveTurn.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Controlable,CoreMatcher.Move)).OnEntityRemoved += OnAcitveAI;
        _pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Controlable, CoreMatcher.Attack)).OnEntityRemoved += OnAcitveAI;
    }

    //激活AI
    private void OnAcitveAI(Group @group, Entity entity, int index, IComponent component)
    {
        if (_pools.input.isActiveTurn)
        {
            Tools.delay(0.05f, () =>
            {
                TestLoadConfig.log.Trace("TurnEnd");
                _pools.input.isActiveTurn = false;
                TestLoadConfig.log.Trace("OnAcitveAI");
                _pools.input.isActiveAI = true;
                //_pools.input.isProcessing = false;
            });
        }
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("TurnSystemv Execute");
        //获取下一步坐标点
        var Pos=PositionGet();
        var dir = new Vector2(_pools.input.input.x, _pools.input.input.y);
        //玩家行为
        RoleBehaviour(Pos, dir);
    }

    //获取下一步坐标点
    Vector2 PositionGet()
    {
        var currentPos = _pools.core.controlableEntity.view.gameObject.transform.position;
        var newX = currentPos.x + _pools.input.input.x;
        var newY = currentPos.y + _pools.input.input.y;
        TestLoadConfig.log.DebugFormat("x: {0},y: {1}", newX, newY);
        return new Vector2(newX, newY);
    }

    //玩家行为
    void RoleBehaviour(Vector2 pos,Vector2 dir)
    {
        //超出边界，直接返回
        if (IsOutGameBoard((int) pos.x, (int) pos.y))
        {
            TestLoadConfig.log.Trace("TurnEnd");
            _pools.input.isActiveTurn = false;
            _pools.input.isProcessing = false;
            TestLoadConfig.log.Trace("TurnSystem Execute IsOutGameBoard.");
            var player = _pools.core.controlableEntity;
            player.position.x = (int) player.view.gameObject.transform.position.x;
            player.position.y = (int) player.view.gameObject.transform.position.y;
            return;
        }

        //获取下一步坐标位置元素
        var entity = _pools.core.gameBoardCache.grid[(int) pos.x, (int) pos.y];
        //根据下一步坐标位置元素，选择行为。
        if (entity != null)
        {
            switch (entity.view.gameObject.tag.ToEnum<GameTags>())
            {
                case GameTags.Food:
                    TestLoadConfig.log.Trace("TurnSystem RoleBehaviour Food.");
                    sound(entity);
                    _pools.core.DestroyEntity(entity);
                    move(pos);
                    updateFood(Res.foodPoints);
                    break;
                case GameTags.Soda:
                    TestLoadConfig.log.Trace("TurnSystem Execute Soda.");
                    sound(entity);
                    _pools.core.DestroyEntity(entity);
                    move(pos);
                    updateHp(Res.sodaPoints);
                    break;
                case GameTags.Enemy:
                    TestLoadConfig.log.Trace("TurnSystem Execute Enemy.");
                    attack(dir);
                    updateFood(-1);
                    enemyGetHit(entity);
                    break;
                case GameTags.Wall:
                    TestLoadConfig.log.Trace("TurnSystem Execute Wall.");
                    attack(dir);
                    updateFood(-1);
                    break;
                case GameTags.Exit:
                    nextLevel();
                    TestLoadConfig.log.Trace("TurnSystem Execute Exit.");
                    break;
                default:
                    throw new Exception(string.Format("Tags not in gameTags:{0}", entity.view.gameObject.tag));
            }
        }
        else
        {
            TestLoadConfig.log.Trace("TurnSystem Execute Null.");
            move(pos);
            updateFood(-1);
        }
    }

    //边界检测
    bool IsOutGameBoard(int x, int y)
    {
        return x < 0 || x > Res.columns - 1 || y < 0 || y > Res.rows - 1;
    }

    //移动
    void move(Vector2 pos)
    {
        TestLoadConfig.log.Trace("Player move");
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_footstep1);
        //建立玩家角色下一步坐标
        _pools.core.controlableEntity.ReplacePosition((int)pos.x, (int)pos.y);
        _pools.core.controlableEntity.IsMove(true);
    }

    //攻击
    void attack(Vector2 dir)
    {
        _pools.core.controlableEntity.AddAttack(dir);        
    }

    //下一关
    void nextLevel()
    {
        _pools.input.ReplaceLevel(_pools.input.level.value + 1);
        if (_pools.input.isActiveTurn)
        {
            Tools.delay(0.1f, () =>
            {
                TestLoadConfig.log.Trace("TurnEnd");
                _pools.input.isActiveTurn = false;
                _pools.input.isProcessing = false;
            });
        }
    }

    //更新食物
    void updateFood(int num)
    {
        var foodEntity = _pools.input.foodPointsEntity;
        foodEntity.ReplaceFoodPoints(foodEntity.foodPoints.value + num);
    }

    //更新血量
    void updateHp(int num)
    {
        var player = _pools.core.controlableEntity;
        player.ReplaceHp(player.hp.curValue + num);
    }

    void enemyGetHit(Entity entity)
    {
        var remainingHp = entity.hp.curValue - Random.Range(1, 3);
        if (remainingHp > 0)
            entity.ReplaceHp(remainingHp);
        else
            entity.IsDestory(true);
    }

    void sound(Entity entity)
    {
        if (entity.view.gameObject.tag == Res.soda)
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_soda2);
        else
            Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit2);
    }

}

