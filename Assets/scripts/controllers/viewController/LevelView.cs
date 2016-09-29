using Entitas;
using UnityEngine.UI;

public class LevelContext : BaseContext
{
    public LevelContext() : base(Res.Level)
    {
    }
}
public class LevelView : BaseView 
{
    Text label;

    void Start()
    {
        label = GetComponentInChildren<Text>();
        Pools.sharedInstance.input.GetGroup(InputMatcher.Level).OnEntityAdded +=
            (group, entity, index, component) =>
            {
                label.text = "Day " + entity.level.value;
            };
    }
}
