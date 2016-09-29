using System;
using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class LevelSystem : IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.Level.OnEntityAdded(); } }
    //坐标集合，避免重复坐标。
    List<Vector2> PositionList = new List<Vector2>();
    Group _items;
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _items = pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Interactive).NoneOf(CoreMatcher.Controlable));
    }
    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("LevelSystem execute.");
        _pools.input.isProcessing = true;

        updateView();
        
        var entity = entities.SingleEntity();
        //清空缓存
        clearGrid();
        //创建场景
        CreateSence(entity.level.value);
        
    }
    void clearGrid()
    {
        foreach (var e in _items.GetEntities())
        {
            _pools.core.DestroyEntity(e);
        }
    }

    //创建场景
    void CreateSence(int value)
    {
        //做一个坐标集合，以防止元素重叠。
        initialPosList();
        //创建玩家角色
        var player = _pools.core.controlableEntity;
        if (player == null)
        {
            _pools.core.CreateEntity()
                .AddPosition(0, 0)
                .IsInteractive(true)
                .IsControlable(true)
                .IsMovable(true)
                .AddAsset(Res.player)
                .AddHp(5);
        }
        else
        {
            player.ReplacePosition(0, 0);
            player.view.gameObject.transform.position = new Vector2(0, 0);
        }
        int enemyCount = (int)Mathf.Log(value, 2f);
        //创建敌人角色
        CreateEntitiesByRandomPos(enemyCount, Res.enemies, (i, tile) =>
        {
            tile.IsMovable(true)
                .AddHp(Random.Range(1, enemyCount+1))
                .IsContainAI(true);
        });
        //创建内部围墙
        CreateEntitiesByRandomPos(Random.Range(Res.wallCountMin,Res.wallCountMax), Res.walls, (i, tile) =>
        {
            //tile.AddHp(3);
        });
        //创建食物
        CreateEntitiesByRandomPos(Random.Range(Res.foodCountMin,Res.foodCountMax), Res.foods, (i, tile) =>
        {

        });
        //创建出口
        _pools.core.CreateEntity()
            .AddPosition(Res.columns - 1, Res.rows - 1)
            .IsInteractive(true)
            .AddAsset(Res.exit);

    }

    //初始化位置列表
    void initialPosList()
    {
        PositionList.Clear();
        for (int x = 1; x < Res.columns - 1; x++)
        {
            for (int y = 1; y < Res.rows - 1; y++)
            {
                PositionList.Add(new Vector2(x, y));
            }
        }
    }


    //创建随机元素
    void CreateEntitiesByRandomPos(int length, string[] resourceArray, Action<int, Entity> postProcess)
    {
        for (int i = 0; i < length; i++)
        {
            int r = Random.Range(0, PositionList.Count);
            var tile = _pools.core.CreateEntity()
                .AddAsset(resourceArray[Random.Range(0, resourceArray.Length)])
                .AddPosition((int)PositionList[r].x, (int)PositionList[r].y)
                .IsInteractive(true);
            PositionList.RemoveAt(r);
            postProcess(i, tile);
        }
    }

    //更新视图
    void updateView()
    {
        Singleton<ContextManager>.Instance.Push(new LevelContext());
        Tools.delay(1f, () =>
        {
            var canvasGroup = Singleton<UIManager>.Instance.GetSingleUI(Res.Level).GetComponent<CanvasGroup>();
            canvasGroup.DOFade(0, 1f).OnComplete(() =>
            {
                Singleton<ContextManager>.Instance.Pop();
                canvasGroup.alpha = 1;
                _pools.input.isProcessing = false;
            });
        });
    }

}

