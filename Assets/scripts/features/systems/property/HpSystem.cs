using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class HpSystem : IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Hp.OnEntityAdded(); } }

    Sprite[] sprites = Resources.LoadAll<Sprite>(Res.tileNumsPath);
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("HpSystem Execute");
        foreach (var e in entities)
        {
            var gameObject = e.view.gameObject.transform.FindChild("Num(Clone)");
            if (gameObject == null)
            {
                Create(e);
                Modify(e);
            }
            else
                Modify(e);

        }

    }

    void Create(Entity e)
    {
        var res = Resources.Load<GameObject>(Res.prefabsPath + Res.num);
        GameObject gameObject = null;
        try
        {
            gameObject = UnityEngine.Object.Instantiate(res);
        }
        catch (Exception)
        {
            TestLoadConfig.log.ErrorFormat("cannot load:{0}.", res.name);
        }
        if (gameObject != null)
        {
            TestLoadConfig.log.Trace("HP SetParent");
            gameObject.transform.position = new Vector2(e.position.x, e.position.y);
            gameObject.transform.SetParent(e.view.gameObject.transform);
        }
    }

    void Modify(Entity e)
    {
        if (e.hp.curValue < 1)
        {
            e.hp.curValue = 0;
            _pools.input.isGameOver = true;
        }
        var gameObject = e.view.gameObject;
        TestLoadConfig.log.Trace(gameObject.name);
        var ten = gameObject.transform.FindChild("Num(Clone)/" + Res.ten).gameObject;
        var single = gameObject.transform.FindChild("Num(Clone)/" + Res.single).gameObject;
        if (e.hp.curValue > 99)
            e.hp.curValue = 99;
        if (e.hp.curValue < 10)
            ten.SetActive(false);
        else
        {
            ten.SetActive(true);
            var tenSprite = ten.GetComponent<SpriteRenderer>();
            tenSprite.sprite = sprites[e.hp.curValue / 10];
            tenSprite.material.color = Color.green;
        }
        var singleSprite = single.GetComponent<SpriteRenderer>();
        singleSprite.sprite = sprites[e.hp.curValue % 10];
        singleSprite.material.color = Color.green;
    }



}

