using UnityEngine;
public class ViewController : MonoBehaviour
{
    void Start()
    {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();

        var contextSingle = Singleton<ContextManager>.Instance;
        contextSingle.Push(new GameStartContext());
    }
}

