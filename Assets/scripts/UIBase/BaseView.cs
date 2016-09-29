using UnityEngine;
public abstract class BaseView:MonoBehaviour
{
    public void OnPause(BaseContext curContext)
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEnter(BaseContext nextContext)
    {
        gameObject.SetActive(true);
    }

    public void OnExit(BaseContext curContext)
    {
        gameObject.SetActive(false);
    }

    public void OnResume(BaseContext lastContext)
    {
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

