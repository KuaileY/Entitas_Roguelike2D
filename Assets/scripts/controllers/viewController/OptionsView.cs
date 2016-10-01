using UnityEngine;
using System.Collections;
using Entitas;

public class OptionsContext : BaseContext
{
    public OptionsContext() : base(Res.Options)
    {
    }
}
public class OptionsView : BaseView 
{
    public void BackCallBack()
    {
        Pools.sharedInstance.input.CreateEntity().AddEfxSound(Res.audios.scavengers_fruit1);
        Singleton<ContextManager>.Instance.Pop();
    }
}
