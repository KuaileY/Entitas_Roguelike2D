using UnityEngine;
using System.Collections;
using Entitas;

public class GameStartContext : BaseContext
{
    public GameStartContext():base(Res.GameStart)
    {
    }
}
public class GameStartView : BaseView 
{
    public void OptionCallBack()
    {
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit1);
        Singleton<ContextManager>.Instance.Push(new OptionsContext());
    }

    public void GameStartCallBack()
    {
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit1);
        Singleton<ContextManager>.Instance.Pop();
        Singleton<ContextManager>.Instance.Push(new FoodContext());

        //创建属性
        FoodSetup();
        //创建场景
        Pools.sharedInstance.input.ReplaceLevel(1);
        //键盘输入打开
        Pools.sharedInstance.input.isProcessing = false;
        //添加背景音乐
        Pools.sharedInstance.input.CreateEntity().AddMusic(Res.audios.scavengers_music);
    }

    void FoodSetup()
    {
        var input = Pools.sharedInstance.input;
        if (input.hasFoodPoints)
            input.DestroyEntity(input.foodPointsEntity);
        input.CreateEntity()
            .AddFoodPoints(Res.initPoints);
    }
}
