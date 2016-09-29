using Entitas;
using UnityEngine;
using UnityEngine.UI;

public class FoodContext : BaseContext
{
    public FoodContext(): base(Res.Food)
    {
    }
}

public class FoodView : BaseView
{
    Text label;
    int oldFood;
    void Start()
    {
        label = GetComponentInChildren<Text>();
        oldFood = Res.initPoints;
        Pools.sharedInstance.input.GetGroup(InputMatcher.FoodPoints).OnEntityAdded +=
            (group, entity, index, component) =>
            {
                var curFood=entity.foodPoints.value;
                if (curFood < 1)
                    Pools.sharedInstance.input.isGameOver = true;
                var differ = oldFood - curFood;
                string perfix;
                if (differ < 0)
                    perfix = string.Format("+{0} ", Mathf.Abs(differ));
                else if (differ > 1)
                    perfix = string.Format("-{0} ", differ);
                else
                    perfix = "";
                label.text = perfix + "Food:" + curFood;
                oldFood = curFood;
            };

    }


}
