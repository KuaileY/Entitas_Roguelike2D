using Entitas;
using UnityEngine;

public sealed class InputSystem : IExecuteSystem,ISetPool
{
    Pool _pool;
    public void Execute()
    {
        int x = 0, y = 0;
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            if (horizontal != 0)
                x = horizontal > 0 ? 1 : -1;
            else
                y = vertical > 0 ? 1 : -1;
            _pool.ReplaceInput(x, y);
        }
    }

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
}

