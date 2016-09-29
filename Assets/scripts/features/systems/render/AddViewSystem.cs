using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddViewSystem:IMultiReactiveSystem,ISetPool
{
    public TriggerOnEvent[] triggers{get{return new TriggerOnEvent[2]
    {
        CoreMatcher.Asset.OnEntityAdded(),
        BoardMatcher.Asset.OnEntityAdded()
    }; } }

    Transform viewContainer; 
    public void SetPool(Pool pool)
    {
        viewContainer = new GameObject(pool.metaData.poolName+"View").transform;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            var res = Resources.Load<GameObject>(Res.prefabsPath + e.asset.name);
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
                e.AddView(gameObject);
                if (e.hasPosition)
                {
                    var pos=e.position;
                    e.view.gameObject.transform.position = new Vector2(pos.x, pos.y);
                }
            }
            gameObject.transform.SetParent(viewContainer, false);
        }
    }




}

