using System.Collections.Generic;
using UnityEngine;

public class ContextManager
{
    private Stack<BaseContext> _contextStack = new Stack<BaseContext>();

    public void Push(BaseContext nextContext)
    {
        if (_contextStack.Count != 0)
        {
            BaseContext curContext = _contextStack.Peek();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnPause(curContext);
        }
        _contextStack.Push(nextContext);
        BaseView nextView = Singleton<UIManager>.Instance.GetSingleUI(nextContext.ViewType).GetComponent<BaseView>();
        nextView.OnEnter(nextContext);
    }

    public void Pop()
    {
        if (_contextStack.Count != 0)
        {
            BaseContext curContext = _contextStack.Peek();
            _contextStack.Pop();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(curContext.ViewType).GetComponent<BaseView>();
            curView.OnExit(curContext);
        }
        if (_contextStack.Count != 0)
        {
            BaseContext lastContext = _contextStack.Peek();
            BaseView curView = Singleton<UIManager>.Instance.GetSingleUI(lastContext.ViewType).GetComponent<BaseView>();
            curView.OnResume(lastContext);
        }
    }
}

