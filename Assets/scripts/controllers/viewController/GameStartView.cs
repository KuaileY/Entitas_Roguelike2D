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
        Singleton<ContextManager>.Instance.Push(new OptionsContext());
    }

    public void GameStartCallBack()
    {
        Singleton<ContextManager>.Instance.Pop();
        Singleton<ContextManager>.Instance.Push(new FoodContext());

        //创建属性
        FoodSetup();
        //创建场景
        Pools.sharedInstance.input.ReplaceLevel(1);
        //键盘输入打开
        Pools.sharedInstance.input.isProcessing = false;
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
