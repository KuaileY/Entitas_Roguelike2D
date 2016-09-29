using UnityEngine;
using System.Collections;

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
        Singleton<ContextManager>.Instance.Pop();
    }
}
