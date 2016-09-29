using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public sealed class FoodPointsComponent : IComponent
{
    public int value;
}

