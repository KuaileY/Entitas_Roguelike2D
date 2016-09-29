using System;
using System.Collections.Generic;
using DG.Tweening;
using Entitas;
using UnityEngine;
using Random = System.Random;

public static class Tools
{
    static Random random = new Random();
    //非锁死延迟执行
    public static T delay<T>(this T t,float time, TweenCallback action)
    {
        int m = 0;
        DOTween.To(() => m, x => m = x, 0, time).OnComplete(action);
        return t;
    }

    public static void delay(float time, TweenCallback action)
    {
        int m = 0;
        DOTween.To(() => m, x => m = x, 0, time).OnComplete(action);
    }

    //从列表中随机选取一个
    public static T Random<T>(this IList<T> list, int start = 0)
    {
        return list[random.Next(start, list.Count)];
    }

    //string转换到enum
    public static T ToEnum<T>(this string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    //判断周围是否有玩家，并返回方向
    public static bool Around(this Entity entity, GameBoardCacheComponent boardCache, out Vector2 dir)
    {
        var pos = entity.view.gameObject.transform.position;
        for (int i = -1; i <= 1; i++)
        {
            if (i != 0 && (pos.x + i >= 0 && pos.x + i < 8))
            {
                if (boardCache.grid[(int)pos.x + i, (int)pos.y] != null)
                    if (boardCache.grid[(int)pos.x + i, (int)pos.y].view.gameObject.tag
                        == GameTags.Player.ToString())
                    {
                        dir = new Vector2(i, 0);
                        return true;
                    }
            }
            if (i != 0 && (pos.y + i >= 0 && pos.y + i < 8))
            {
                if (boardCache.grid[(int)pos.x, (int)pos.y + i] != null)
                    if (boardCache.grid[(int)pos.x, (int)pos.y + i].view.gameObject.tag
                        == GameTags.Player.ToString())
                    {
                        dir = new Vector2(0, i);
                        return true;
                    }
            }
        }
        dir = new Vector2(0, 0);
        return false;
    }

    //判断周围是否为空
    public static bool Around(this Entity entity, GameBoardCacheComponent boardCache)
    {
        var pos = entity.view.gameObject.transform.position;
        for (int i = -1; i <= 1; i++)
        {
            if (i != 0 && (pos.x + i >= 0 && pos.x + i < 8))
            {
                if (boardCache.grid[(int)pos.x + i, (int)pos.y] == null)
                    return false;
            }
            if (i != 0 && (pos.y + i >= 0 && pos.y + i < 8))
            {
                if (boardCache.grid[(int)pos.x, (int)pos.y + i] == null)
                    return false;
            }
        }
        return true;
    }


}


