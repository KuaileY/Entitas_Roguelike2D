using UnityEngine;
using System.Collections;
using Entitas;
using UnityEngine.UI;

public class GameOvertContext : BaseContext
{
    public GameOvertContext(): base(Res.GameOver)
    {
    }
}
public class GameOverView : BaseView 
{
    public void PlayerAgainCallBack()
    {
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit1);
        Singleton<ContextManager>.Instance.Pop();
        Singleton<ContextManager>.Instance.Push(new FoodContext());
        var input = Pools.sharedInstance.input;
        //创建属性
        FoodSetup(input);
        //删除GameOver
        input.isGameOver = false;
        //创建场景
        input.ReplaceLevel(1);
        //键盘输入打开
        input.isProcessing = false;
        //添加背景音乐
        Pools.sharedInstance.input.CreateEntity().AddMusic(Res.audios.scavengers_music);
    }

    void FoodSetup(Pool input)
    {
        if (input.hasFoodPoints)
            input.DestroyEntity(input.foodPointsEntity);
        input.CreateEntity()
            .AddFoodPoints(Res.initPoints);
    }

    public void ExitCallBack()
    {
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit1);
        Singleton<ContextManager>.Instance.Pop();
        Singleton<ContextManager>.Instance.Push(new GameStartContext());
        Pools.sharedInstance.input.isGameOver = false;
    }
}
